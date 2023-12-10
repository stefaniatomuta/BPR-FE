using BPR.Analysis.Models;
using BPR.Model.Enums;
using BPR.Model.Results;

namespace BPR.Analysis.Helpers;

internal class ViolationFactory
{
    internal static Violation CreateDependencyViolation(UsingDirective directive, string componentName)
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
}
