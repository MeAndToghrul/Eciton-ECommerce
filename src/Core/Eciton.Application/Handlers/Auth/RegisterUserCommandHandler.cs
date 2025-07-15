using Eciton.Application.Commands.Auth;
using Eciton.Application.ResponceObject;
using MediatR;

namespace Eciton.Application.Handlers.Auth;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response>
{
    public async Task<Response> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

    }
}
