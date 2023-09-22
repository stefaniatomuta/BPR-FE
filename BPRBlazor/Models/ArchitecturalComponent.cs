using System.ComponentModel.DataAnnotations;

namespace BPRBlazor.Models;

public class ArchitecturalComponent
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please give the component a name")]
    public string Name { get; set; } = string.Empty;
}
