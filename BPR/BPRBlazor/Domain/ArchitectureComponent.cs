namespace BPRBlazor.Domain;

public class ArchitectureComponent
{
    public string Name { get; set; }
    public IEnumerable<ArchitectureComponent> Dependencies { get; set; }
    public IEnumerable<ProjectComponent> Components { get; set; }
}