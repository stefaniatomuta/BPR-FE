using BPRBE.Models.Results.Enums;

namespace BPRBE.Models.Results;

public record ViolationModel(int Id, string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code);
