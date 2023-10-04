using System.ComponentModel.DataAnnotations;

namespace BPRBlazor.Models;

public class ArchitecturalComponent
{
    public int Id { get; set; }

    public string Style { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please give the component a name")]
    public string Name { get; set; } = string.Empty;

    public List<ArchitecturalComponent> Dependencies { get; set; } = new();

    public List<Namespace> NamespaceComponents { get; set; } = new();
}
