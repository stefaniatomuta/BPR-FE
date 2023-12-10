using BPR.Model.Enums;

namespace BPRBlazor.ViewModels;

public class RuleViewModel
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public bool IsChecked { get; set; }
    public RuleType RuleType { get; set; }

    public RuleViewModel(string name, string? description, RuleType ruleType)
    {
        Name = name;
        Description = description;
        RuleType = ruleType;
    }
}