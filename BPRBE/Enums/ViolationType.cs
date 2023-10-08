namespace BPR.Models.Enums;

public enum ViolationType
{
    Unknown = 0,
    ForbiddenDependencyDirection = 1,
    ForbiddenDependency = 2,
    MismatchedNamespace = 3
}
