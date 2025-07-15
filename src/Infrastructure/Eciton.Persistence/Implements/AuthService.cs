using AutoMapper;
using Eciton.Application.Abstractions;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.Exceptions.Commons;
using Eciton.Application.Helpers;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Domain.Entities.Identity;
using Eciton.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Eciton.Persistence.Implements;
public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly PasswordService _passwordService;
    public AuthService(AppDbContext appDbContext,IMapper mapper, PasswordService passwordService)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _passwordService = passwordService;
    }

    public Task<Response> LoginAsync(LoginDTO user)
    {
        throw new NotImplementedException();
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

        return new Response(ResponseStatusCode.Success, "User registered successfully.");
    }
}
