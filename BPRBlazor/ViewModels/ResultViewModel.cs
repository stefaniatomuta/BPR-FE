using BPR.Mediator.Enums;
using BPR.Persistence.Models;

namespace BPRBlazor.ViewModels;

public class ResultViewModel
{
    public Guid Id { get; set; }
    public DateTime ResultStart { get; set; }
    public DateTime ResultEnd { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public double Score { get; set; }
    public List<ViolationViewModel> Violations { get; set; } = new();
}