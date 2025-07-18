using Eciton.Domain.Entities.Identity;
namespace Eciton.Application.Abstractions;
public interface ITokenService
{        
    string GenerateToken(AppUser user);
    string GenerateEmailVerificationToken(string userId, string email);
    (string userId, string email) ValidateEmailVerificationToken(string token);
}
