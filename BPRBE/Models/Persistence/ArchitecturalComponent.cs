
namespace BPRBE.Models.Persistence;

public class ArchitecturalComponent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<int> Dependencies { get; set; }
}