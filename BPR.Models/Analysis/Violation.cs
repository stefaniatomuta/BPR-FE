using BPR.Models.Enums;

namespace BPR.Models.Analysis; 

public class Violation {
    public string Name { get; set; } = null!;

    public ViolationType Type { get; set; }
    public string Description { get; set; } = null!;
    public ViolationSeverity Severity { get; set; }
    public string Code { get; set; } = null!;
}