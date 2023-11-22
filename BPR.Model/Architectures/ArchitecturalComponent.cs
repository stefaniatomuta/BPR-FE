namespace BPR.Model.Architectures;

public class ArchitecturalComponent
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitecturalDependency> Dependencies { get; set; } = new List<ArchitecturalDependency>();
    public IList<NamespaceModel> NamespaceComponents { get; set; } = new List<NamespaceModel>();
    public Position Position { get; set; } = default!;
    public bool IsOpen { get; set; }
}