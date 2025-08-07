using Eciton.Application.Abstractions.Repositories;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Infrastructure.Context;
using MongoDB.Driver;
namespace Eciton.Infrastructure.Repositories.Read;

public class CategoryReadRepository : ICategoryReadRepository
{
    private readonly IMongoCollection<CategoryReadModel> _collection;

    public CategoryReadRepository(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.Categories;
    }

    public async Task<Response<List<CategoryReadModel>>> GetAllAsync()
    {
        var categories = await _collection.Find(_ => true).ToListAsync();

        if (categories == null || !categories.Any())
        {
            return new Response<List<CategoryReadModel>>(ResponseStatusCode.NotFound, "No categories found.");
        }

        var result =  categories.Select(c => new CategoryReadModel
        {
            Id = c.Id,
            Name = c.Name,
            CategoryImage = c.CategoryImage
        }).ToList();

        return new Response<List<CategoryReadModel>>(ResponseStatusCode.Success, result);
    }

    public async Task<Response<CategoryReadModel>> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new Response<CategoryReadModel>(ResponseStatusCode.Error, "Category ID cannot be null or empty.");
        }

        var category = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (category == null)
        {
            return new Response<CategoryReadModel>(ResponseStatusCode.NotFound, "Category not found.");
        }

        return new Response<CategoryReadModel>(ResponseStatusCode.Success, category);
    }

    public async Task<Response<CategoryReadModel>> GetByNameAsync(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            return new Response<CategoryReadModel>(ResponseStatusCode.Error, "Category name cannot be null or empty.");
        }

        var category = await _collection.Find(x => x.Name == name).FirstOrDefaultAsync();

        if (category == null)
        {
            return new Response<CategoryReadModel>(ResponseStatusCode.NotFound, "Category not found.");
        }

        return new Response<CategoryReadModel>(ResponseStatusCode.Success,category);
    }
}
