using BPR.Model.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Persistence.Collections;

public class RuleCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public RuleType RuleType { get; set; }
}