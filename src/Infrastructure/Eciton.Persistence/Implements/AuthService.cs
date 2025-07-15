using Eciton.Application.Abstractions;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.Exceptions.Commons;
using Eciton.Application.ResponceObject;
using Eciton.Domain.Entities.Identity;
using Eciton.Persistence.Contexts;

namespace Eciton.Persistence.Implements;
public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    public AuthService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public Task<Response> RegisterAsync(RegisterDTO user)
    {
        var email = user.Email.ToLower().Trim();
        
        if( _appDbContext.AppUsers.Any(u => u.Email == email))
        {
            throw new ExistException<AppUser>("Email has already taken");
        }
    }
}
