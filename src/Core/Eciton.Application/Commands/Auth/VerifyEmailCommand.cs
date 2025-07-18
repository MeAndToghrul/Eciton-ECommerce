using Eciton.Application.ResponceObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Commands.Auth
{
    public class VerifyEmailCommand : IRequest<Response>
    {
        public string Token { get; set; } = null!;
    }
}
