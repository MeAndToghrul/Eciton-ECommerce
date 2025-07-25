using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;
using MediatR;

namespace Eciton.Application.Handlers.Auth;
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response>
{
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Response> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userIp = request.Model.UserIp;
        return await _authService.LoginAsync(request.Model);
    }
}
