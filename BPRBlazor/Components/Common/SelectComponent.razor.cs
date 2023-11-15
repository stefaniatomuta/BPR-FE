using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Common;

public partial class SelectComponent : ComponentBase
{
    [Parameter, EditorRequired] public string Id { get; set; } = string.Empty;
    [Parameter, EditorRequired] public string Label { get; set; } = string.Empty;
    [Parameter] public string? Description { get; set; }

    private bool _value;

    [Parameter, EditorRequired]
    public bool Value
    {
        get => _value;
        set
        {
            if (_value == value) return;
            _value = value;
            ValueChanged.InvokeAsync(_value);
        }
    }

    [Parameter] public EventCallback<bool> ValueChanged { get; set; }
}
