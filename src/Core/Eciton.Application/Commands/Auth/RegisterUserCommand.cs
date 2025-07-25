using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;
using MediatR;

namespace Eciton.Application.Commands.Auth;
public class RegisterUserCommand : IRequest<Response>
{
    public RegisterDTO Model { get; set; } 
}
