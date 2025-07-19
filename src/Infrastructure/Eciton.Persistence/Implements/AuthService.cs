using AutoMapper;
using Eciton.Application.Abstractions;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.Events;
using Eciton.Application.Helpers;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Domain.Entities.Identity;
using Eciton.Domain.Settings;
using Eciton.Infrastructure.Mongo.ReadModels;
using Eciton.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Text;

namespace Eciton.Persistence.Implements;
public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly PasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IEventBus _eventBus;
    public AuthService(AppDbContext appDbContext,
        IMapper mapper,
        PasswordService passwordService,
        ITokenService tokenService,
        IEmailService emailService,
        IEventBus eventBus)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _emailService = emailService;
        _eventBus = eventBus;
    }

    public async Task<Response> LoginAsync(LoginDTO user)
    {
        var email = user.Email.ToLower().Trim();

        var appUser = await _appDbContext.AppUsers
            .Where(u => u.Email == email)
            .Include(x => x.Role)
            .FirstOrDefaultAsync();

        if (appUser == null)
        {
            return new Response(ResponseStatusCode.Error, "Email və ya şifrə yanlışdır.");
        }

        bool passwordValid = _passwordService.VerifyPassword(appUser.PasswordHash, user.Password);

        if (!passwordValid)
        {
            return new Response(ResponseStatusCode.Error, "Email və ya şifrə yanlışdır.");
        }

        var token = _tokenService.GenerateToken(appUser);

        return new Response(ResponseStatusCode.Success, token);
    }



    public async Task<Response> RegisterAsync(RegisterDTO user)
    {
        var email = user.Email.ToLower().Trim();

        if (await _appDbContext.AppUsers.AnyAsync(u => u.Email == email))
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
            appUser.RoleId,
            appUser.IsEmailConfirmed
        ));

        var verificationToken = _tokenService.GenerateEmailVerificationToken(appUser.Id, appUser.Email);

        await _emailService.SendVerificationEmailAsync(appUser.Email, verificationToken);

        return new Response(ResponseStatusCode.Success, "User registered successfully. Please check your email to verify your account.");

    }

    public async Task<Response> ResendEmailVerificationAsync(string email)
    {
        email = email.ToLower().Trim();

        var user = await _appDbContext.AppUsers
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return new Response(ResponseStatusCode.Error, "İstifadəçi tapılmadı.");

        if (user.IsEmailConfirmed)
            return new Response(ResponseStatusCode.Success, "Email artıq təsdiqlənmişdir.");

        var token = _tokenService.GenerateEmailVerificationToken(user.Id, user.Email);

        await _emailService.SendVerificationEmailAsync(user.Email, token);

        return new Response(ResponseStatusCode.Success, "Email təsdiqləmə linki yenidən göndərildi.");
    }


    public async Task<Response> VerifyEmailAsync(string token)
    {
        try
        {
            var (userId, email) = _tokenService.ValidateEmailVerificationToken(token);

            var user = await _appDbContext.AppUsers.FindAsync(userId);
            if (user == null)
                return new Response(ResponseStatusCode.Error, "İstifadəçi tapılmadı və ya token yanlışdır.");

            if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
                return new Response(ResponseStatusCode.Error, "Token uyğun email ilə uyğunlaşmır.");

            if (user.IsEmailConfirmed)
                return new Response(ResponseStatusCode.Success, "Email artıq təsdiqlənmişdir.");

            user.IsEmailConfirmed = true;
            await _appDbContext.SaveChangesAsync();

            await _eventBus.PublishAsync(new UserEmailConfirmedEvent(user.Id));

            return new Response(ResponseStatusCode.Success, "Email uğurla təsdiqləndi.");
        }
        catch (SecurityTokenException ex)
        {
            return new Response(ResponseStatusCode.Error, $"Token xətası: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new Response(ResponseStatusCode.Error, "Email təsdiqləmə zamanı xəta baş verdi.");
        }

    }





}
