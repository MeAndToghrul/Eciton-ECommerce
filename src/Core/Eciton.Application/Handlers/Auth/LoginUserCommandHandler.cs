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
        return await _authService.LoginAsync(new LoginDTO
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe
        });
    }
}
