using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Persistence.Models;

public class ArchitecturalModelCollection
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitecturalComponentCollection> Components { get; set; } = default!;
}