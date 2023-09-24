namespace BPRBlazor.Pages.Results;

public partial class Violation : ComponentBase
{
    private string SeverityCssClass => Model.Severity.ToString().ToLower();
    
    [Parameter]
    public ViolationModel Model { get; set; } = default!;
}