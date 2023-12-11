using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Common;

public partial class SelectRuleComponent : ComponentBase
{
    [Parameter, EditorRequired]
    public EventCallback<RuleViewModel> OnChange { get; set; }

    private List<RuleViewModel>? _rules;

    protected override async Task OnInitializedAsync()
    {
        _rules = await GetRulesAsync();
        
        if (_rules != null)
        {
            foreach (var rule in _rules)
            {
                await OnChange.InvokeAsync(rule);
            }
        }
    }

    private async Task<List<RuleViewModel>> GetRulesAsync()
    {
        var rules = await Service.GetRulesAsync();
        return rules.Select(rule => new RuleViewModel(rule.Name, rule.Description, rule.RuleType))
                    .OrderBy(rule => rule.Name)
                    .ToList();
    }

    private async Task CheckboxChangedAsync(ChangeEventArgs e, RuleViewModel item)
    {
        item.IsChecked = (bool)(e.Value ?? false);
        await OnChange.InvokeAsync(item);
    }
}