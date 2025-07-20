using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Commands.Auth
{
    public class ChangePasswordCommand : IRequest<Response>
    {
        public ChangePasswordDTO Model { get; set; }
    }
}
