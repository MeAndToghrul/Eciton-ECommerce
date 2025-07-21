using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.ResponceObject;
using MediatR;

namespace Eciton.Application.Handlers.Auth;

public class ConfirmResetPasswordCommandHandler : IRequestHandler<ConfirmResetPasswordCommand, Response>
{
    private readonly IAuthService _authService;

    public ConfirmResetPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Response> Handle(ConfirmResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ConfirmResetPasswordAsync(request.Model);
    }
}
