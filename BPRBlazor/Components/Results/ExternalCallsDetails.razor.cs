using BPR.Model.Results.External;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results;

public partial class ExternalCallsDetails : ComponentBase
{
    [Parameter]
    public List<Dictionary<string, EndOfLifeFramework>> EndOfLifeFrameworks { get; set; } = new();

    [Parameter] 
    public Dictionary<string, ExternalApiMetrics> ExternalApisCalls { get; set; } = new();
}