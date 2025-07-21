using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Infrastructure.Context;
using Eciton.Infrastructure.Mongo.ReadModels;
using MongoDB.Driver;

namespace Eciton.Infrastructure.EventHandlers;
public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{
    private readonly IMongoCollection<UserReadModel> _collection;

    public UserRegisteredEventHandler(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.Users;
    }

    public async Task HandleAsync(UserRegisteredEvent @event)
    {
        var user = new UserReadModel
        {
            Id = @event.Id,
            FullName = @event.FullName,
            Email = @event.Email,
            RoleId = @event.RoleId,
            IsEmailConfirmed = @event.IsEmailConfirmed,
        };

        await _collection.InsertOneAsync(user);
    }
}
