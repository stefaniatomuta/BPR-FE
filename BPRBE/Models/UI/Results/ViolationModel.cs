using BPRBE.Models.UI.Results.Enums;

namespace BPRBE.Models.UI.Results;

public record ViolationModel(int Id, string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code);
