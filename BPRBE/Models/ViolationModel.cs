using BPRBE.Enums;

namespace BPRBE.Models;

public record ViolationModel(string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code,string File) { }


