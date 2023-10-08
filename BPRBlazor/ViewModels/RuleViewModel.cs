namespace BPRBlazor.ViewModels;

public class RuleViewModel
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsChecked { get; set; }
}