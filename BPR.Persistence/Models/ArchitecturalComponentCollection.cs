using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Persistence.Models;

public class ArchitecturalComponentCollection
{
    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitecturalComponentCollection> Dependencies { get; set; } = new List<ArchitecturalComponentCollection>();
    public Position Position { get; set; } = default!;
}