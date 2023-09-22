using BPRBE.UserInterface.Enums;

namespace BPRBE.UserInterface.Models;

public record ViolationModel(int Id, string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code);
