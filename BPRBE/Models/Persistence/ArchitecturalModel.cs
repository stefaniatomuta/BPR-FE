using MongoDB.Bson.Serialization.Attributes;

namespace BPRBE.Models.Persistence;

public class ArchitecturalModel
{
    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<ArchitecturalComponent> Components { get; set; }
}