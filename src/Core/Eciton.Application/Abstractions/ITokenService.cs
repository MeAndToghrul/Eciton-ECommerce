using Eciton.Infrastructure.Mongo.ReadModels;
namespace Eciton.Application.Abstractions;
public interface ITokenService
{        
    string GenerateToken(UserReadModel user);
}
