using Eciton.Application.DTOs.Category;
using Eciton.Application.ResponceObject;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Eciton.Application.Commands.Category;
public class CreateCategoryCommand : IRequest<Response>
{
    public CreateCategoryDTO Model { get; set; }
}
