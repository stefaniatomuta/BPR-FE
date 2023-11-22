using BPR.Model.Enums;

namespace BPRBlazor.ViewModels;

public class RuleViewModel
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public bool IsChecked { get; set; }
    public ViolationType ViolationType { get; set; }

    public RuleViewModel(string name, string? description, ViolationType violationType)
    {
        Name = name;
        Description = description;
        ViolationType = violationType;
    }
}