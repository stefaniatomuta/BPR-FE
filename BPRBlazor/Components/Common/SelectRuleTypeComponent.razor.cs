using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Common;

public partial class SelectRuleTypeComponent : ComponentBase
{
    [Parameter, EditorRequired]
    public EventCallback<RuleTypeViewModel> OnChange { get; set; }

    [Parameter, EditorRequired] 
    public List<RuleTypeViewModel>? RuleTypes { get; set; }

    private async Task CheckboxChanged(ChangeEventArgs e, RuleTypeViewModel item)
    {
        item.IsChecked = (bool)(e.Value ?? false);
        await OnChange.InvokeAsync(item);
    }
}