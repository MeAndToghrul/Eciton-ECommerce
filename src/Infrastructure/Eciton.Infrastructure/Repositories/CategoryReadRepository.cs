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
}
