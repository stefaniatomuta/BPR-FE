using BPRBE.Models.Persistence;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class SelectRuleComponent : ComponentBase
{
    [Parameter]
    public EventCallback<RuleViewModel> Value { get; set; }

    public List<RuleViewModel> Rules = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Rules = await GetRulesAsync();
        foreach (var rule in Rules)
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
            Description = rule.Description!,
            IsChecked = (rule.Name == "Dependency")
        }).ToList();
    }

    public async Task CheckboxChanged(ChangeEventArgs e, RuleViewModel item)
    {
        item.IsChecked = (bool)(e.Value ?? false);
        await Value.InvokeAsync(item);
    }
}