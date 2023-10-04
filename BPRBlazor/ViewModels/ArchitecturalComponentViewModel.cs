using System.ComponentModel.DataAnnotations;

namespace BPRBlazor.ViewModels;

public class ArchitecturalComponentViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please give the component a name")]
    public string Name { get; set; } = string.Empty;

    public List<ArchitecturalComponentViewModel> Dependencies { get; set; } = new();

    public List<NamespaceViewModel> NamespaceComponents { get; set; } = new();
    
    public override string ToString()
    {
        return $"\tName: {Name}, Dependencies: \n\t\t'{string.Join(", ", Dependencies.Select(x => x.Name))}'";
    }
}
