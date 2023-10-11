using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Persistence.Models;

public class RuleCollection
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}