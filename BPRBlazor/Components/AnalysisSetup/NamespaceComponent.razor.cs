using BPRBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class NamespaceComponent : ComponentBase
{
    [Parameter]
    public Namespace Namespace { get; set; } = default!;
}