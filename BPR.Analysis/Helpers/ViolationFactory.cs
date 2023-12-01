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
            Type = ViolationType.ForbiddenDependency,
            Description = $"'{directive.Using}' cannot be in '{directive.FilePath}'. Component '{directive.ComponentName}' in namespace '{componentName}' cannot have this dependency",
            Severity = ViolationSeverity.Major,
            Code = directive.Using,
            File = directive.File
        };
    }
}
