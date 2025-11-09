using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Application.ReadModels;
using Eciton.Infrastructure.Context;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Eciton.Infrastructure.EventHandlers
{
    public class ProductCreatedEventHandler : IEventHandler<ProductCreatedEvent>
    {
        private readonly IMongoCollection<ProductReadModel> _collection;

        public ProductCreatedEventHandler(MongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.Products;
        }

        public async Task HandleAsync(ProductCreatedEvent @event)
        {
            var product = new ProductReadModel
            {
                Id = @event.Id,
                Brand = @event.Brand,
                Price = @event.Price,
                CategoryName = @event.CategoryName,
                StockQuantity= @event.StockQuantity,
                Model = @event.Model,
                ProductImage = @event.ProductImage
            };

            await _collection.InsertOneAsync(product);
        }
    }
}
