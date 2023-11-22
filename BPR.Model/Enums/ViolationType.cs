using System.ComponentModel;

namespace BPR.Model.Enums;

public enum ViolationType
{
    [Description("Unknown violation type")] 
    Unknown = 0,
    [Description("Forbidden dependency")] 
    ForbiddenDependency = 1,
    [Description("Mismatched namespace")] 
    MismatchedNamespace = 2
}