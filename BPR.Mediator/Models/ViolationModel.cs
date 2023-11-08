using BPR.Mediator.Enums;

namespace BPR.Mediator.Models;

public record ViolationModel(ViolationType Type, string Description, ViolationSeverity Severity, string Code, string File)
{
}
