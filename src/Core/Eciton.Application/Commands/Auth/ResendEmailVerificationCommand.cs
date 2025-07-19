using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Commands.Auth;
public class ResendEmailVerificationCommand : IRequest<Response>
{
    public string email { get; set; } = null!;
}
