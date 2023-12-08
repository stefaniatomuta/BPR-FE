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
    [Description("Conditional statements")] 
    ConditionalStatements,
    [Description("Solution metrics")] 
    SolutionMetrics,
    [Description("External calls")] 
    ExternalCalls,
    [Description("Code similarity")] 
    CodeSimilarity,
    [Description("Technical debt classification")]
    TechnicalDebtClassification
}