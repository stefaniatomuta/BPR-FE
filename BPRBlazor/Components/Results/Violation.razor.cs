using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results;

public partial class Violation : ComponentBase
{
    [Parameter]
    public ViolationViewModel Model { get; set; } = default!;
    
    private string _severityCssClass => Model.Severity.ToString()?.ToLower() ?? string.Empty;
}