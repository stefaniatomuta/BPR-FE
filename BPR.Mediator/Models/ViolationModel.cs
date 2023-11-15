using BPR.Mediator.Enums;

namespace BPR.Mediator.Models;

public class ViolationModel
{
    public ViolationType Type { get; set; }
    public string Description { get; set; } = default!;
    public ViolationSeverity Severity { get; set; }
    public string Code { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
}
