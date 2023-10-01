using MongoDB.Bson.Serialization.Attributes;

namespace BPRBE.Models.Persistence;

public class MongoArchitecturalModel
{
    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<MongoArchitecturalComponent> Components { get; set; } = default!;
}