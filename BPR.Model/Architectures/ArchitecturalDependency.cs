namespace BPR.Model.Architectures;

public class ArchitecturalDependency
{
    public int Id { get; set; }
    public bool IsOpen { get; set; }
    public bool IsViolation { get; set; }
}