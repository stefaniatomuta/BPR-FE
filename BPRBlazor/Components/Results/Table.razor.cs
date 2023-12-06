using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results;

public partial class Table<T> : ComponentBase
{
    [Parameter, EditorRequired]
    public Dictionary<string, T> Data { get; set; } = new();

    [Parameter, EditorRequired]
    public string HeaderOne { get; set; } = string.Empty;

    [Parameter, EditorRequired]
    public string HeaderTwo { get; set; } = string.Empty;

    [Parameter]
    public bool PushLastColumnRight { get; set; }
}
