using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results;

public partial class Violation : ComponentBase
{
    private string SeverityCssClass => Model.Severity.ToString()?.ToLower() ?? string.Empty;
    
    [Parameter]
    public ViolationViewModel Model { get; set; } = default!;
}