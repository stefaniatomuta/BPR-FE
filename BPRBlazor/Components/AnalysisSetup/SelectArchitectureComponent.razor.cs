using BPRBE.Models.Persistence;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class SelectArchitectureComponent : ComponentBase
{
    private string? SelectedArchitecturalModel { get; set; }
    [Parameter]
    public EventCallback<string> ArchitecturalModelChanged { get; set; }

    public IList<MongoArchitecturalModel> ArchitecturalOptions = new List<MongoArchitecturalModel>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        // ArchitecturalOptions = await GetArchitecturalModels();
    }

    private async Task<IList<MongoArchitecturalModel>> GetArchitecturalModels()
    {
        return await DependencyService.GetArchitecturalModelsAsync();
    }
    public async Task OnSelectedValueChanged(ChangeEventArgs e)
    {
        foreach (var ar in ArchitecturalOptions)
        {
            if (ar.Name.Equals(e.Value))
            {
                SelectedArchitecturalModel = ar.Name;
                await ArchitecturalModelChanged.InvokeAsync(SelectedArchitecturalModel);
            }
        }
    }
}