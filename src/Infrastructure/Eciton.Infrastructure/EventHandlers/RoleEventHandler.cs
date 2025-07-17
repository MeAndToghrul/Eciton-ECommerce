using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Application.ReadModels;
using Eciton.Infrastructure.Context;
using MongoDB.Driver;

namespace Eciton.Infrastructure.EventHandlers;
public class RoleEventHandler : IEventHandler<RoleCreatedEvent>
{
    private readonly IMongoCollection<RoleReadModel> _collection;
    public RoleEventHandler(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.Roles;
    }
    public async Task HandleAsync(RoleCreatedEvent @event)
    {
        var role = new RoleReadModel
        {
            Id = @event.Id,
            Name = @event.Name,
        };
        await _collection.InsertOneAsync(role);
    }
}