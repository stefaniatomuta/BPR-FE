using BPRBE.Models.Persistence;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class SelectArchitectureComponent : ComponentBase
{
    [Parameter]
    public EventCallback<ArchitecturalModel> ArchitecturalModelChanged { get; set; }

    public IList<ArchitecturalModel> ArchitecturalOptions = new List<ArchitecturalModel>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        // ArchitecturalOptions = await GetArchitecturalModels();
    }

    private async Task<IList<ArchitecturalModel>> GetArchitecturalModels()
    {
        return await DependencyService.GetArchitecturalModelsAsync();
    }
    public async Task OnSelectedValueChanged(ChangeEventArgs e)
    {
        foreach (var ar in ArchitecturalOptions)
        {
            if (ar.Name.Equals(e.Value))
            {
                await ArchitecturalModelChanged.InvokeAsync(ar);
            }
        }
    }
}