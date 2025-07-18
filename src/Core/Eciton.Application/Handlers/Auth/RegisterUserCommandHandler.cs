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
        var registerResponse = await _authService.RegisterAsync(new RegisterDTO
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        });

        return registerResponse;
    }
}
