using BPRBE.Services.Enums;

namespace BPRBE.Services.Models;

public record ViolationModel(string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code,string File) { }


