using Eciton.Application.Abstractions;
using Eciton.Application.Commands.CategoryField;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateCategoryFieldCommandHandler : IRequestHandler<CreateCategoryFieldCommand, Response>
{
    private readonly ICategoryService _categoryService;
    private readonly ICategoryFieldService _categoryFieldService;

    public CreateCategoryFieldCommandHandler(
        ICategoryService categoryService,
        ICategoryFieldService categoryFieldService)
    {
        _categoryService = categoryService;
        _categoryFieldService = categoryFieldService;
    }

    public async Task<Response> Handle(CreateCategoryFieldCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _categoryService.ExistsAsync(request.Model.CategoryId);
        if (!categoryExists)
        {
            return new Response(ResponseStatusCode.Error,"Belirtilen kategori bulunamadı.");
        }

        var result = await _categoryFieldService.CreateAsync(request.Model);

        return result;
    }
}
