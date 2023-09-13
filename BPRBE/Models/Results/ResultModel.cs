namespace BPRBE.Models.Results;

public record ResultModel(double Score, IEnumerable<ViolationModel> Violations);
