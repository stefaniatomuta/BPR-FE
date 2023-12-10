using BPR.Model.Enums;

namespace BPR.Model.Results;

public class Violation
{
    public RuleType Type { get; set; }
    public string Description { get; set; } = default!;
    public ViolationSeverity? Severity { get; set; }
    public string Code { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
}