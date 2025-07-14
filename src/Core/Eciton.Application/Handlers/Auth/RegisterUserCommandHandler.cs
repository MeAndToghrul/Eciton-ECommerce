using Eciton.Application.Commands.Auth;
using MediatR;

namespace Eciton.Application.Handlers.Auth;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    public Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
