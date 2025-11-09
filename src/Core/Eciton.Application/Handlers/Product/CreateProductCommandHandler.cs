using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Product;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eciton.Application.Handlers.Product
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response>
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CreateProductCommandHandler(
            ICategoryService categoryService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<Response> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _categoryService.ExistsAsync(request.Model.CategoryId);
            if (!categoryExists)
            {
                return new Response(ResponseStatusCode.Error, "Belirtilen kategori bulunamadı.");
            }

            var result = await _productService.CreateAsync(request.Model);

            return result;
        }
    }
}
