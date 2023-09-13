namespace BPRBE.Models.UI.Results;

public record ResultModel(double Score, IEnumerable<ViolationModel> Violations);
