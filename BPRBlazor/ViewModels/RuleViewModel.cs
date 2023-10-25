namespace BPRBlazor.ViewModels;

public class RuleViewModel
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public bool IsChecked { get; set; }

    public RuleViewModel(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}