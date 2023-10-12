using BPR.Persistence.Models;

namespace BPR.Mediator.Models;

public class ArchitecturalComponent
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<ArchitecturalComponent> Dependencies { get; set; } = new List<ArchitecturalComponent>();
    public Position Position { get; set; } = default!;
}