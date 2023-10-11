using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Common;

public partial class SelectRuleComponent : ComponentBase
{
    [Parameter]
    public EventCallback<RuleViewModel> Value { get; set; }

    private List<RuleViewModel>? _rules;

    protected override async Task OnInitializedAsync()
    {
        _rules = await GetRulesAsync();
        foreach (var rule in _rules)
        {
            await Value.InvokeAsync(rule);
        }
    }

    private async Task<List<RuleViewModel>> GetRulesAsync()
    {
        var rules = await Service.GetRulesAsync();
        return rules.Select(rule => new RuleViewModel()
        {
            Name = rule.Name,
            Description = rule.Description,
            IsChecked = (rule.Name == "Dependency")
        }).ToList();
    }

    private async Task CheckboxChanged(ChangeEventArgs e, RuleViewModel item)
    {
        item.IsChecked = (bool)(e.Value ?? false);
        await Value.InvokeAsync(item);
    }
}