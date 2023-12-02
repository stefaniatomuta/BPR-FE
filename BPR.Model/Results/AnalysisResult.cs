using BPR.Model.Enums;

namespace BPR.Model.Results;

public class AnalysisResult
{
    public Guid Id { get; set; }
    public double Score { get; set; }
    public DateTime ResultStart { get; set; }
    public DateTime ResultEnd { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public List<Violation> Violations { get; set; } = new();
    public ExtendedAnalysisResults? ExtendedAnalysisResults { get; set; }
}