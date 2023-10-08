namespace BPR.Models.Analysis;

public record ResultModel(double Score, IEnumerable<ViolationModel> Violations);
