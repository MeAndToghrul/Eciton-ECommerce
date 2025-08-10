using Eciton.Application.Abstractions.Repositories;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Infrastructure.Context;
using MongoDB.Driver;
namespace Eciton.Infrastructure.Repositories.Read;
public class CategoryFieldReadRepository : ICategoryFieldReadRepository
{
    private readonly IMongoCollection<CategoryFieldReadModel> _collection;

    public CategoryFieldReadRepository(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.CategoryFields;
    }

    public async Task<Response<List<CategoryFieldReadModel>>> GetAllAsync()
    {
        var fields = await _collection.Find(_ => true).ToListAsync();

        if (fields == null || !fields.Any())
        {
            return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.NotFound, "No category fields found.");
        }

        var result = fields.Select(f => new CategoryFieldReadModel
        {
            Id = f.Id,
            CategoryId = f.CategoryId,
            FieldName = f.FieldName,
            DataType = f.DataType
        }).ToList();

        return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.Success, result);
    }

    public async Task<Response<CategoryFieldReadModel>> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new Response<CategoryFieldReadModel>(ResponseStatusCode.Error, "CategoryField ID cannot be null or empty.");
        }

        var field = await _collection.Find(f => f.Id == id).FirstOrDefaultAsync();

        if (field == null)
        {
            return new Response<CategoryFieldReadModel>(ResponseStatusCode.NotFound, "Category field not found.");
        }

        return new Response<CategoryFieldReadModel>(ResponseStatusCode.Success, field);
    }

    public async Task<Response<List<CategoryFieldReadModel>>> GetByCategoryIdAsync(string categoryId)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
        {
            return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.Error, "Category ID cannot be null or empty.");
        }

        var fields = await _collection.Find(f => f.CategoryId == categoryId).ToListAsync();

        if (fields == null || !fields.Any())
        {
            return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.NotFound, "No category fields found for this category.");
        }

        var result = fields.Select(f => new CategoryFieldReadModel
        {
            Id = f.Id,
            CategoryId = f.CategoryId,
            FieldName = f.FieldName,
            DataType = f.DataType
        }).ToList();

        return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.Success, result);
    }
}
