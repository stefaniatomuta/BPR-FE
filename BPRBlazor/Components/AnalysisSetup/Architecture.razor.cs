using BPRBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class Architecture : ComponentBase
{
    [Parameter]
    public ArchitecturalModel ArchitecturalModel { get; set; } = default!;
}