using BPR.Mediator.Enums;

namespace BPR.Mediator.Models;

public record ViolationModel(string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code,string File) { }


