using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.ResponceObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Handlers.Auth
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Response>
    {
        private readonly IAuthService _authService;

        public VerifyEmailCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            return await _authService.VerifyEmailAsync(request.Token);
        }
    }
}
