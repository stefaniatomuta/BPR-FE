using BPRBE.Enums;

namespace BPRBE.Models;

public record ViolationModel(int Id, string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code);
