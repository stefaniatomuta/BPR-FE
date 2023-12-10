namespace BPR.Model.Architectures;

public class ArchitectureComponent
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitectureDependency> Dependencies { get; set; } = new List<ArchitectureDependency>();
    public IList<NamespaceModel> NamespaceComponents { get; set; } = new List<NamespaceModel>();
    public Position Position { get; set; } = default!;
}