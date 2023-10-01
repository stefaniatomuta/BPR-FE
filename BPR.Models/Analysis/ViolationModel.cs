using BPR.Models.Enums;

namespace BPR.Models.Analysis;

// public record ViolationModel(int Id, string Name, ViolationType Type, string Description, ViolationSeverity Severity, string Code) {
//    
// }

public class ViolationModel {
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ViolationType Type { get; set; }
    public string Description { get; set; } = null!;
    public ViolationSeverity Severity { get; set; }
    public string Code { get; set; } = null!;
}
