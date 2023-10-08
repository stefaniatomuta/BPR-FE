using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPRBE.Models.Persistence;

public class ArchitecturalModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitecturalComponent> Components { get; set; } = default!;
}