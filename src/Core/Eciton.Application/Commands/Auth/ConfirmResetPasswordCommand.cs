using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Commands.Auth;
public class ConfirmResetPasswordCommand : IRequest<Response>
{
    public ResetPasswordDTO Model { get; set; }
}
