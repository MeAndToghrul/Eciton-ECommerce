using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.ResponceObject;
using MediatR;
namespace Eciton.Application.Handlers.Auth;
public class ResetPasswordRequestCommandHandler : IRequestHandler<ResetPasswordRequestCommand, Response>
{
    private readonly IAuthService _authService;

    public ResetPasswordRequestCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Response> Handle(ResetPasswordRequestCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ResetPasswordAsync(request.Email);
    }
}
