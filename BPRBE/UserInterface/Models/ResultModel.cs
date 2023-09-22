namespace BPRBE.UserInterface.Models;

public record ResultModel(double Score, IEnumerable<ViolationModel> Violations);
