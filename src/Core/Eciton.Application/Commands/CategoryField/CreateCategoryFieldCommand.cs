using Eciton.Application.DTOs.CategoryField;
using Eciton.Application.ResponceObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Commands.CategoryField
{
    public class CreateCategoryFieldCommand : IRequest<Response>
    {
        public CreateCategoryFieldDTO Model { get; set; }
    }
}
