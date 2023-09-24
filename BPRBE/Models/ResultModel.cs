namespace BPRBE.Models;

public record ResultModel(double Score, IEnumerable<ViolationModel> Violations);
