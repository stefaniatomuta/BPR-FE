using BPR.Model.Enums;
using BPR.Model.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPR.Persistence.Collections;

public class ResultCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ResultStart { get; set; }
    public DateTime ResultEnd { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public ArchitectureModelsCollection? ArchitectureModel { get; set; }
    public List<RuleType> RuleTypes { get; set; } = new();
    public List<Violation> Violations { get; set; } = new();
    public ExtendedAnalysisResults? ExtendedAnalysisResults { get; set; }
}