namespace BPRBlazor.ViewModels;

public class RuleViewModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsChecked { get; set; }
}