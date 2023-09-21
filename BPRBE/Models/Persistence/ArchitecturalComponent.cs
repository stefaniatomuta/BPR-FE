using MongoDB.Bson.Serialization.Attributes;

namespace BPRBE.Models.Persistence;

public class ArchitecturalComponent
{
    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<int> Dependencies { get; set; }
}