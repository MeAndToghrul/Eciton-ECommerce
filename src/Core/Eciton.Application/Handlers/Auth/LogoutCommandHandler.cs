using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Handlers.Auth;
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Response>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Response> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LogOutAsync();
    }
}
