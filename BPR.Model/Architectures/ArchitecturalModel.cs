namespace BPR.Model.Architectures;

public class ArchitecturalModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitecturalComponent> Components { get; set; } = default!;
}