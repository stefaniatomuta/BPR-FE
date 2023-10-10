using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPRBE.Models.Persistence;

public class Rule
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}