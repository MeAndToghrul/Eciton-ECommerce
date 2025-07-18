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
    public class ResendEmailVerificationCommandHandler : IRequestHandler<ResendEmailVerificationCommand, Response>
    {
        private readonly IAuthService _authService;

        public ResendEmailVerificationCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response> Handle(ResendEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ResendEmailVerificationAsync(request.email);
        }
    }
}
