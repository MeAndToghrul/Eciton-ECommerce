using Eciton.Domain.Entities.Identity;
namespace Eciton.Application.Abstractions;
public interface ITokenService
{        
    string GenerateToken(AppUser user);
}
