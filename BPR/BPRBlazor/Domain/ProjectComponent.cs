namespace BPRBlazor.Domain;

public class ProjectComponent
{
    public string Name { get; set; }
    public IEnumerable<ProjectComponent> Dependencies { get; set; }
}