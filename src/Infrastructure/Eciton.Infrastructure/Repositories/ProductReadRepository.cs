using Eciton.Application.Abstractions.Repositories;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Infrastructure.Context;
using MongoDB.Driver;
namespace Eciton.Infrastructure.Repositories.Read;
public class ProductReadRepository : IProductReadRepository
{
    private readonly IMongoCollection<ProductReadModel> _collection;

    public ProductReadRepository(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.Products;
    }

    public async Task<Response<ProductReadModel>> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new Response<ProductReadModel>(ResponseStatusCode.Error, "Product ID cannot be null or empty.");

        var product = await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (product == null)
            return new Response<ProductReadModel>(ResponseStatusCode.NotFound, "Product not found.");

        return new Response<ProductReadModel>(ResponseStatusCode.Success, product);
    }

    public async Task<Response<List<ProductReadModel>>> GetByNameAsync(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
            return new Response<List<ProductReadModel>>(ResponseStatusCode.Error, "Product name cannot be null or empty.");

        var filter = Builders<ProductReadModel>.Filter.Regex(p => p.Brand, new MongoDB.Bson.BsonRegularExpression(brand, "i"));

        var products = await _collection.Find(filter).ToListAsync();

        if (products == null || !products.Any())
            return new Response<List<ProductReadModel>>(ResponseStatusCode.NotFound, "No products found with this brand.");

        return new Response<List<ProductReadModel>>(ResponseStatusCode.Success, products);
    }

    public async Task<Response<List<ProductReadModel>>> GetAllAsync()
    {
        var products = await _collection.Find(_ => true).ToListAsync();

        if (products == null || !products.Any())
            return new Response<List<ProductReadModel>>(ResponseStatusCode.NotFound, "No products found.");

        return new Response<List<ProductReadModel>>(ResponseStatusCode.Success, products);
    }
}
