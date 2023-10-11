using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace BPRBlazor.ViewModels;

public class ArchitecturalModelViewModel
{
    public Guid Id { get; set; } = new Guid();
    
    [Required(ErrorMessage = "Please give the model a name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "The model must consist of at least {1} component")]
    [ValidateComplexType]
    public List<ArchitecturalComponentViewModel> Components { get; set; } = new();

    public override string ToString()
    {
        return $"Name: {Name}, Components: \n{string.Join('\n', Components.Select(x => x.ToString()))}";
    }

    public List<ArchitecturalComponentViewModel> GetDependentComponents(ArchitecturalComponentViewModel component)
    {
        return Components.Where(dependentComponent => dependentComponent.Dependencies.
                Any(dependency => component == dependency)).ToList();
    }
}
