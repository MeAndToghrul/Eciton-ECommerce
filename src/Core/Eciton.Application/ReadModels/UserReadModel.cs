using Eciton.Application.ReadModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Eciton.Infrastructure.Mongo.ReadModels;
public class UserReadModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = null!;

    [BsonElement("fullName")]
    public string FullName { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("roleName")]
    public string RoleName { get; set; } = null!;

    [BsonElement("isEmailConfirmed")]
    public bool IsEmailConfirmed { get; set; }
}

