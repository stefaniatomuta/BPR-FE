using System.ComponentModel.DataAnnotations;

namespace BPRBlazor.Models;

public class ArchitecturalModel
{
    [Required(ErrorMessage = "Please give the model a name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "The model must consist of at least {1} component")]
    [ValidateComplexType]
    public List<ArchitecturalComponent> Components { get; set; } = new();

    public override string ToString()
    {
        return $"Name: {Name}, Components: \n{string.Join('\n', Components.Select(x => x.ToString()))}";
    }
}
