namespace BPR.Model.Architectures;

public class ArchitectureModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitectureComponent> Components { get; set; } = default!;
}