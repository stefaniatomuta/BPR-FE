using BPRBE.Services.Models;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results;

public partial class Violation : ComponentBase
{
    private string SeverityCssClass => Model.Severity.ToString().ToLower();
    
    [Parameter]
    public ViolationModel Model { get; set; } = default!;
}