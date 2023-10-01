
namespace BPRBE.Models.Persistence;

public class MongoArchitecturalComponent
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IList<int>? Dependencies { get; set; }
}