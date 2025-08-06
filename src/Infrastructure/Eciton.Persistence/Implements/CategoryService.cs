using AutoMapper;
using Eciton.Application.Abstractions;
using Eciton.Application.Abstractions.Repositories;
using Eciton.Application.DTOs.Category;
using Eciton.Application.Events;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Domain.Entities.Entity;
using Eciton.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Eciton.Persistence.Implements;
public class CategoryService : ICategoryService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly IStorageService _storageService;
    private readonly IEventBus _eventBus;
    private readonly ICategoryReadRepository _categoryReadRepository;

    public CategoryService(AppDbContext appDbContext, IMapper mapper, IStorageService storageService,IEventBus eventBus,ICategoryReadRepository categoryReadRepository)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _storageService = storageService;
        _eventBus = eventBus;
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<Response> CreateAsync(CreateCategoryDTO category)
    {
        var existingCategory = await _appDbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == category.Name);

        if (existingCategory != null)
            return new Response(ResponseStatusCode.Error, $"{category.Name} already exists.");

        var newCategory = _mapper.Map<Category>(category);

        if (category.CategoryImage != null && category.CategoryImage.Length > 0)
        {
            using var stream = category.CategoryImage.OpenReadStream();
            string imageUrl = await _storageService.UploadFileAsync(stream, category.CategoryImage.FileName, "categories");
            newCategory.CategoryImage = imageUrl;
        }

        await _appDbContext.Categories.AddAsync(newCategory);
        await _appDbContext.SaveChangesAsync();

        await _eventBus.PublishAsync(new CategoryCreatedEvent(
            newCategory.Id,
            newCategory.Name,
            newCategory.CategoryImage));

        return new Response(ResponseStatusCode.Success, "Category created successfully.");
    }

    public async Task<Response<CategoryReadModel>> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new Response<CategoryReadModel>(ResponseStatusCode.Error, "Id is required.");
        }

        return await _categoryReadRepository.GetByIdAsync(id);
    }
}
