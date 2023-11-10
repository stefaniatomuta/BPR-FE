namespace BPR.Persistence.Models;

public class Violation
{
    public int Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Severity { get; set; }
    public string Code { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
}