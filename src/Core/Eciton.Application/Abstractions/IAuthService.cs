using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;

namespace Eciton.Application.Abstractions;
public interface IAuthService
{
    Task<Response> RegisterAsync(RegisterDTO user);
}
