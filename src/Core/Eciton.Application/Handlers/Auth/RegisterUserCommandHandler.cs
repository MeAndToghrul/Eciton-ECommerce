using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;
using MediatR;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response>
{
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Response> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerResponse = await _authService.RegisterAsync(request.Model);
        return registerResponse;
    }
}
