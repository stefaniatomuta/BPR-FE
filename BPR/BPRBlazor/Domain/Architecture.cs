namespace BPRBlazor.Domain;

public class Architecture
{
    public string Name { get; set; }
    public IEnumerable<ArchitectureComponent> Components { get; set; }
}