using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Persistence.Models;

public class ResultCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public double Score { get; set; }
    public DateTime ResultStart { get; set; }
    public DateTime ResultEnd { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public List<Violation> Violations { get; set; } = new();
}