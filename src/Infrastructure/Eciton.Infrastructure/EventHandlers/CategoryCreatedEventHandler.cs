using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Application.ReadModels;
using Eciton.Infrastructure.Context;
using MongoDB.Driver;

namespace Eciton.Infrastructure.EventHandlers;
public class CategoryCreatedEventHandler : IEventHandler<CategoryCreatedEvent>
{
    private readonly IMongoCollection<CategoryReadModel> _collection;

    public CategoryCreatedEventHandler(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.Categories;
    }

    public async Task HandleAsync(CategoryCreatedEvent @event)
    {
        var category = new CategoryReadModel
        {
            Id = @event.Id,
            Name = @event.Name,
            CategoryImage = @event.CategoryImage
        };

        await _collection.InsertOneAsync(category);
    }
}
