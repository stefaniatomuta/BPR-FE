using BPR.Mediator.Utils;
using BPR.Model.Enums;

namespace BPRBlazor.ViewModels;

public class RuleTypeViewModel
{
    public RuleType RuleType { get; init; }
    public string Name { get; init; }
    public bool IsChecked { get; set; }

    public RuleTypeViewModel(RuleType ruleType)
    {
        RuleType = ruleType;
        Name = EnumExtensions.GetDescription(ruleType);
        IsChecked = true;
    }
}