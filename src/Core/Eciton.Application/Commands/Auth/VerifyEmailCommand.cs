using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Commands.Auth;
public class VerifyEmailCommand : IRequest<Response>
{
    public string Token { get; set; } = null!;
}
