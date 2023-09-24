using BPRBE.Models;

namespace BPRBlazor.Shared;

public partial class Violation : ComponentBase
{
    [Parameter]
    public ViolationModel Model { get; set; } = default!;

    private string SeverityCssClass => Model.Severity.ToString().ToLower();
}