using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Models.Persistence;

public class ArchitecturalComponent
{
    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<int>? Dependencies { get; set; }
}