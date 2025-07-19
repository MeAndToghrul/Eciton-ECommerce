using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Commands.Auth;
public class ResetPasswordRequestCommand : IRequest<Response>
{
    public string Email { get; set; } = string.Empty;
}
