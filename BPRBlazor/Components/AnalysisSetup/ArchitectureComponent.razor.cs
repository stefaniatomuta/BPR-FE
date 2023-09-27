using BPRBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class ArchitectureComponent : ComponentBase
{
    [Parameter]
    public ArchitecturalComponent ArchitecturalComponent { get; set; } = default!;
}