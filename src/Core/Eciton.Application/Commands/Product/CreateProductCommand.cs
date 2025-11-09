using Eciton.Application.DTOs.Product;
using Eciton.Application.ResponceObject;
using MediatR;

namespace Eciton.Application.Commands.Product;
public class CreateProductCommand : IRequest<Response>
{
    public CreateProductDTO Model { get; set; }
}
