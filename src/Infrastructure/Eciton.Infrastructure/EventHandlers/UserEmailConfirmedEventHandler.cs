using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Infrastructure.Context;
using Eciton.Infrastructure.Mongo.ReadModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Infrastructure.EventHandlers
{
    public class UserEmailConfirmedEventHandler : IEventHandler<UserEmailConfirmedEvent>
    {
        private readonly IMongoCollection<UserReadModel> _collection;

        public UserEmailConfirmedEventHandler(MongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.Users;
        }

        public async Task HandleAsync(UserEmailConfirmedEvent @event)
        {
            var filter = Builders<UserReadModel>.Filter.Eq(u => u.Id, @event.UserId);
            var update = Builders<UserReadModel>.Update.Set(u => u.IsEmailConfirmed, true);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
