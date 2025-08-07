using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Application.ReadModels;
using Eciton.Infrastructure.Context;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Eciton.Infrastructure.EventHandlers
{
    public class CategoryFieldCreatedEventHandler : IEventHandler<CategoryFieldCreatedEvent>
    {
        private readonly IMongoCollection<CategoryFieldReadModel> _collection;

        public CategoryFieldCreatedEventHandler(MongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.CategoryFields;
        }

        public async Task HandleAsync(CategoryFieldCreatedEvent @event)
        {
            var categoryField = new CategoryFieldReadModel
            {
                Id = @event.Id,
                CategoryId = @event.CategoryId,
                FieldName = @event.FieldName,
                DataType = @event.DataType
            };

            await _collection.InsertOneAsync(categoryField);
        }
    }
}
