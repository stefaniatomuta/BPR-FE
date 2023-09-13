using BPRBE.Models.Results;

namespace BPRBlazor.Shared;

public partial class Violation : ComponentBase
{
    [Parameter]
    public ViolationModel Model { get; set; }
}