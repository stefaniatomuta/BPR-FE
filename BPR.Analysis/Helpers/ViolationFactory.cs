using BPR.Analysis.Models;
using BPR.Model.Enums;
using BPR.Model.Results;

namespace BPR.Analysis.Helpers;

public static class ViolationFactory
{
    public static Violation CreateDependencyViolation(UsingDirective directive, string componentName)
    {
        return new Violation
        {
            Type = RuleType.ForbiddenDependency,
            Description = $"'{directive.Using}' cannot be in '{directive.FilePath}'. Namespace '{directive.ComponentName}' in component '{componentName}' cannot have this dependency",
            Severity = ViolationSeverity.Major,
            Code = directive.Using,
            File = directive.File
        };
    }
    
    public static Violation CreateNamespaceViolation(NamespaceDirective directive)
    {
        return new Violation
        {
            File = directive.File,
            Severity = ViolationSeverity.Minor,
            Code = directive.Namespace,
            Description = $"Namespace '{directive.Namespace}' in '{directive.File}' does not match",
            Type = RuleType.MismatchedNamespace
        };
    }
}
