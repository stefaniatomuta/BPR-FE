using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Common;

public partial class SelectViolationTypeComponent : ComponentBase
{
    [Parameter, EditorRequired]
    public EventCallback<ViolationTypeViewModel> OnChange { get; set; }

    [Parameter, EditorRequired] 
    public List<ViolationTypeViewModel>? Violations { get; set; }

    private async Task CheckboxChanged(ChangeEventArgs e, ViolationTypeViewModel item)
    {
        item.IsChecked = (bool)(e.Value ?? false);
        await OnChange.InvokeAsync(item);
    }
}