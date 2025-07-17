using AutoMapper;
using Eciton.Application.Abstractions;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.Events;
using Eciton.Application.Helpers;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Domain.Entities.Identity;
using Eciton.Infrastructure.Mongo.ReadModels;
using Eciton.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

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

        await _eventBus.PublishAsync(new UserRegisteredEvent
            (
            appUser.Id,
            appUser.FullName,
            appUser.Email,
            appUser.RoleId,
            appUser.IsEmailConfirmed
            ));

        MailMessage msg = new MailMessage
        {
            Subject = "Welcome to Eciton",
            Body = $"Hello {appUser.FullName},\n\nThank you for registering on Eciton. Your account has been created successfully.\n\nBest regards,\nEciton Team",
            To = { new MailAddress(appUser.Email, appUser.FullName) },
        };

        await _emailService.SendEmailAsync(msg);

        return new Response(ResponseStatusCode.Success, "User registered successfully.");
    }
}
