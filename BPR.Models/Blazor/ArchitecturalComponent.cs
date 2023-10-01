using System.ComponentModel.DataAnnotations;

namespace BPR.Models.Blazor;

public class ArchitecturalComponent
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please give the component a name")]
    public string Name { get; set; } = string.Empty;

    public List<ArchitecturalComponent> Dependencies { get; set; } = new();

    public List<Namespace> NamespaceComponents { get; set; } = new();
    
    public override string ToString()
    {
        return $"\tName: {Name}, Dependencies: \n\t\t'{string.Join(", ", Dependencies.Select(x => x.Name))}'";
    }
}
