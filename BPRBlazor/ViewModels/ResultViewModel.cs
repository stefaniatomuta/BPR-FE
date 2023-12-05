using BPR.Model.Enums;
using BPR.Model.Results;

namespace BPRBlazor.ViewModels;

public class ResultViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ResultStart { get; set; }
    public DateTime ResultEnd { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public List<ViolationViewModel> Violations { get; set; } = new();
    public ExtendedAnalysisResults? ExtendedAnalysisResults { get; set; }
}