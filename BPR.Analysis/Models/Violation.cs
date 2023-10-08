using BPR.Analysis.Enums;

namespace BPR.Analysis.Models; 

public class Violation {
    public string Name { get; set; } = null!;

    public ViolationType Type { get; set; }
    public string Description { get; set; } = null!;
    public ViolationSeverity Severity { get; set; }
    public string Code { get; set; } = null!;
    public string File { get; set; } = null!;
}