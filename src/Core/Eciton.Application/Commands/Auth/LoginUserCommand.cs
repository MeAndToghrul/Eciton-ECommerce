using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Commands.Auth;
public class LoginUserCommand : IRequest<Response>
{
    public LoginDTO Model { get; set; } = null!;
}
