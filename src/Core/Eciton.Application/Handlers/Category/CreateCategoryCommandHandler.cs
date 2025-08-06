using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Category;
using Eciton.Application.ResponceObject;
using MediatR;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response>
{
    private readonly ICategoryService _categoryService;

    public CreateCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<Response> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.CreateAsync(request.Model);
        return result;
    }
}
