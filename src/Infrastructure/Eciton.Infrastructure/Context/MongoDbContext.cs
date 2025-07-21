using Eciton.Application.ReadModels;
using Eciton.Domain.Settings;
using Eciton.Infrastructure.Mongo.ReadModels;
using MongoDB.Driver;

namespace Eciton.Infrastructure.Context;
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<UserReadModel> Users =>
        _database.GetCollection<UserReadModel>("Users");


    public IMongoCollection<RoleReadModel> Roles =>
        _database.GetCollection<RoleReadModel>("Roles");
}
