using System.ComponentModel;

namespace BPR.Model.Enums;

public enum RuleType
{
    [Description("Unknown rule type")] 
    Unknown,
    [Description("Dependency")] 
    ForbiddenDependency,
    [Description("Namespace")] 
    MismatchedNamespace,
    [Description("Conditional statements")] 
    ConditionalStatements,
    [Description("Solution metrics")] 
    SolutionMetrics,
    [Description("External calls")] 
    ExternalCalls,
    [Description("Code similarity")] 
    CodeSimilarity
}