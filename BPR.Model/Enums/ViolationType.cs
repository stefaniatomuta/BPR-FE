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
    [Description("Conditional Statements")] 
    ConditionalStatements,
    [Description("Solution Metrics")] 
    SolutionMetrics,
    [Description("External Calls")] 
    ExternalCalls,
    [Description("Code Similarity")] 
    CodeSimilarity
}