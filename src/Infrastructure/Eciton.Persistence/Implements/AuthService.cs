using AutoMapper;
using Eciton.Application.Abstractions;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.Events;
using Eciton.Application.ExternalServices;
using Eciton.Application.Helpers;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Domain.Entities.Identity;
using Eciton.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Eciton.Persistence.Implements;
public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly UserService _userService;
    private readonly PasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IEventBus _eventBus;
    private readonly ICacheService _cacheService;
    private readonly IRateLimitService _rateLimitService;
    public AuthService(AppDbContext appDbContext,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        UserService userService,
        PasswordService passwordService,
        ITokenService tokenService,
        IEmailService emailService,
        IEventBus eventBus,
        ICacheService cacheService,
        IRateLimitService rateLimitService)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _emailService = emailService;
        _eventBus = eventBus;
        _cacheService = cacheService;
        _rateLimitService = rateLimitService;
    }

    public async Task<Response> LoginAsync(LoginDTO user)
    {

        if (await _rateLimitService.IsBlocked(user.UserIp))
        {
            return new Response(ResponseStatusCode.Error, "E gijdillaq sikdirde");
        }

        var normalizedEmail = user.Email.ToUpper().Trim();

        var appUser = await _appDbContext.AppUsers
            .Where(u => u.NormalizedEmail == normalizedEmail)
            .Include(x => x.Role)
            .FirstOrDefaultAsync();

        if (appUser == null)
        {
            return new Response(ResponseStatusCode.Error, "Invalid email or password.");
        }

        bool passwordValid = _passwordService.VerifyPassword(appUser.PasswordHash, user.Password);

        if (appUser.AccessFailedCount >= appUser.MaxFailedAccessAttempts && appUser.LockoutEnd != null)
        {
            await _rateLimitService.RegisterFailedAttempt(user.UserIp);

            // bununla bagli email gonderilecek
            
            return new Response(ResponseStatusCode.Error, "Account is locked due to too many failed login attempts. Please contact support to unlock your account.");
        }


        if (!passwordValid)
        {
            await _rateLimitService.RegisterFailedAttempt(user.UserIp);

            appUser.AccessFailedCount++;

            appUser.LastFailedAttempt = DateTime.UtcNow;

            if (appUser.AccessFailedCount >= appUser.MaxFailedAccessAttempts)
            {
                appUser.LockoutEnd = DateTime.UtcNow.AddMinutes(5);
            }

            await _appDbContext.SaveChangesAsync();
            return new Response(ResponseStatusCode.Error, "Invalid email or password.");
        }


        var tokenId = Guid.NewGuid().ToString();

        var token = _tokenService.GenerateToken(appUser, tokenId);

        await _cacheService.SetAsync($"UserToken_{tokenId}", appUser.Id, 3600);

        return new Response(ResponseStatusCode.Success, token);
    }
    public async Task<Response> RegisterAsync(RegisterDTO user)
    {
        var normalizedEmail = user.Email.ToUpper().Trim();

        if (await _appDbContext.AppUsers.AnyAsync(u => u.NormalizedEmail == normalizedEmail))
        {
            return new Response(ResponseStatusCode.Error, "Email already exists.");
        }

        if (user.Password != user.ConfirmPassword)
        {
            return new Response(ResponseStatusCode.Error, "Passwords do not match.");
        }

        var appUser = _mapper.Map<AppUser>(user);
        appUser.PasswordHash = _passwordService.HashPassword(user.Password);

        var defaultRoleId = await _appDbContext.AppRoles
            .Where(r => r.Name == "Guest")
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        if (defaultRoleId == null)
        {
            return new Response(ResponseStatusCode.Error, "Default role 'Guest' not found.");
        }

        appUser.RoleId = defaultRoleId!;
        await _appDbContext.AddAsync(appUser);
        await _appDbContext.SaveChangesAsync();

        await _eventBus.PublishAsync(new UserRegisteredEvent(
            appUser.Id,
            appUser.FullName,
            appUser.Email,
            "Guest",
            appUser.IsEmailConfirmed
        ));

        var verificationToken = _tokenService.GenerateEmailVerificationToken(appUser.Id, appUser.Email);

        await _cacheService.SetAsync($"EmailVerificationToken_{appUser.Id}", verificationToken, 3600);

        await _emailService.SendVerificationEmailAsync(appUser.Email, verificationToken);

        return new Response(ResponseStatusCode.Success, "User registered successfully. Please check your email to verify your account.");
    }
    public async Task<Response> ResendEmailVerificationAsync(string email)
    {
        email = email.ToLower().Trim();

        var user = await _appDbContext.AppUsers
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return new Response(ResponseStatusCode.Error, "User not found.");

        if (user.IsEmailConfirmed)
            return new Response(ResponseStatusCode.Success, "Email has already been verified.");

        var token = _tokenService.GenerateEmailVerificationToken(user.Id, user.Email);

        if (await _cacheService.IsExistsAsync($"EmailVerificationToken_{user.Id}"))
        {
            return new Response(ResponseStatusCode.Error, "A verification email has already been sent. Please check your inbox.");
        }

        await _cacheService.SetAsync($"EmailVerificationToken_{user.Id}", token, 3600);

        await _emailService.SendVerificationEmailAsync(user.Email, token);

        return new Response(ResponseStatusCode.Success, "Verification email has been resent.");
    }
    public async Task<Response> VerifyEmailAsync(string token)
    {
        try
        {
            var (userId, email) = _tokenService.ValidateEmailVerificationToken(token);

            var cacheData = await _cacheService.GetAsync<string>($"EmailVerificationToken_{userId}");

            if (cacheData == null || cacheData != token)
                return new Response(ResponseStatusCode.Error, "Invalid or expired token.");

            var user = await _appDbContext.AppUsers.FindAsync(userId);

            if (user == null)
                return new Response(ResponseStatusCode.Error, "User not found or invalid token.");

            if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
                return new Response(ResponseStatusCode.Error, "Token does not match the user's email.");

            if (user.IsEmailConfirmed)
                return new Response(ResponseStatusCode.Success, "Email has already been verified.");

            user.IsEmailConfirmed = true;

            user.RoleId = await _appDbContext.AppRoles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync() ?? user.RoleId;

            await _appDbContext.SaveChangesAsync();

            _cacheService.Delete($"EmailVerificationToken_{userId}");

            await _eventBus.PublishAsync(new UserEmailConfirmedEvent(user.Id));

            return new Response(ResponseStatusCode.Success, "Email has been successfully verified.");
        }
        catch (SecurityTokenException ex)
        {
            return new Response(ResponseStatusCode.Error, $"Token error: {ex.Message}");
        }
        catch (Exception)
        {
            return new Response(ResponseStatusCode.Error, "An error occurred during email verification.");
        }
    }
    public async Task<Response> ResetPasswordAsync(string email)
    {
        email = email.Trim().ToLower();

        var user = await _appDbContext.AppUsers
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user == null)
            return new Response(ResponseStatusCode.Error, "User not found.");

        if (!user.IsEmailConfirmed)
            return new Response(ResponseStatusCode.EmailNotConfirmed, "Email not confirmed.");

        var token = _tokenService.GeneratePasswordResetToken(user.Id, user.Email);

        await _emailService.SendPasswordResetEmailAsync(user.Email, token);

        await _cacheService.SetAsync($"PasswordResetToken_{user.Id}", token, 1800);

        return new Response(ResponseStatusCode.Success, "Password reset link has been sent to your email.");
    }
    public async Task<Response> ConfirmResetPasswordAsync(ResetPasswordDTO model)
    {
        try
        {
            var (userId, email) = _tokenService.ValidatePasswordResetToken(model.Token);

            var user = await _appDbContext.AppUsers.FindAsync(userId);

            var cacheData = await _cacheService.GetAsync<string>($"PasswordResetToken_{userId}");

            if (cacheData == null || cacheData != model.Token)
                return new Response(ResponseStatusCode.Error, "Invalid or expired token.");

            if (user == null)
                return new Response(ResponseStatusCode.Error, "User not found.");

            if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
                return new Response(ResponseStatusCode.Error, "Invalid token.");

            if (model.NewPassword != model.ConfirmPassword)
                return new Response(ResponseStatusCode.Error, "New passwords do not match.");

            user.PasswordHash = _passwordService.HashPassword(model.NewPassword);
            await _appDbContext.SaveChangesAsync();

            _cacheService.Delete($"PasswordResetToken_{userId}");

            return new Response(ResponseStatusCode.Success, "Password has been reset successfully.");
        }
        catch (SecurityTokenException ex)
        {
            return new Response(ResponseStatusCode.Error, $"Token error: {ex.Message}");
        }
        catch (Exception)
        {
            return new Response(ResponseStatusCode.Error, "An error occurred during password reset.");
        }
    }
    public async Task<Response> LogOutAsync()
    {
        var tokenId = _userService.GetCurrentTokenId();

        if (string.IsNullOrEmpty(tokenId))
            return new Response(ResponseStatusCode.Error, "Token not identified.");

        var exists = await _cacheService.IsExistsAsync($"UserToken_{tokenId}");
        if (!exists)
            return new Response(ResponseStatusCode.Error, "Token not found or already logged out.");

        _cacheService.Delete($"UserToken_{tokenId}");

        return new Response(ResponseStatusCode.Success, "Logged out successfully.");
    }
    public async Task<Response> ChangePasswordAsync(ChangePasswordDTO model)
    {
        var userId = _userService.GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return new Response(ResponseStatusCode.Error, "User not identified.");
        }
        var user = await _appDbContext.AppUsers.FindAsync((userId));
        if (user == null)
        {
            return new Response(ResponseStatusCode.Error, "User not found.");
        }
        if (!_passwordService.VerifyPassword(user.PasswordHash, model.OldPassword))
        {
            return new Response(ResponseStatusCode.Error, "Old password is incorrect.");
        }
        if (_passwordService.VerifyPassword(user.PasswordHash, model.NewPassword))
        {
            return new Response(ResponseStatusCode.Error, "New password cannot be the same as the old password.");
        }
        if (model.NewPassword != model.ConfirmNewPassword)
        {
            return new Response(ResponseStatusCode.Error, "New passwords do not match.");
        }
        user.PasswordHash = _passwordService.HashPassword(model.NewPassword);
        await _appDbContext.SaveChangesAsync();
        return new Response(ResponseStatusCode.Success, "Password changed successfully.");

    }
    public async Task RefreshLockoutEndAsync()
    {
        await _appDbContext.AppUsers
            .Where(x => x.LockoutEnd != null && x.LockoutEnd <= DateTime.UtcNow)
            .ExecuteUpdateAsync(update => update
                .SetProperty(x => x.LockoutEnd, x => null)
                .SetProperty(x => x.AccessFailedCount, x => 0)
            );
    }
}
