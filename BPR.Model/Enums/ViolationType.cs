using System.ComponentModel;

namespace BPR.Model.Enums;

public enum ViolationType
{
    [Description("Unknown violation type")] 
    Unknown,
    [Description("Forbidden dependency")] 
    ForbiddenDependency,
    [Description("Mismatched namespace")] 
    MismatchedNamespace,
    ConditionalStatements,
    SolutionMetrics,
    ExternalCalls,
    CodeSimilarity
}