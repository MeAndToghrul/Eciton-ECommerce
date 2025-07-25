using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Commands.Auth;
public class ChangePasswordCommand : IRequest<Response>
{
    public ChangePasswordDTO Model { get; set; }
}
