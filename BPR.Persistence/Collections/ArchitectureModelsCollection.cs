using BPR.Model.Architectures;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Persistence.Collections;

public class ArchitectureModelsCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitectureComponent> Components { get; set; } = default!;
}