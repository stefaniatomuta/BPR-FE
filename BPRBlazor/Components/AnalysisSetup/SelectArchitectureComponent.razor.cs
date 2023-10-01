using BPRBE.Models.Persistence;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class SelectArchitectureComponent : ComponentBase
{
    public string SelectedArchitecturalModel;

    public IList<ArchitecturalModel> architecturalOption = new List<ArchitecturalModel>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        architecturalOption = await GetArchitecturalModels();
    }

    private async Task<IList<ArchitecturalModel>> GetArchitecturalModels()
    {
        return await DependencyRepository.GetArchitecturalModelsAsync();
    }
    public async Task OnSelectedValueChangedAsync(ChangeEventArgs e)
    {
        foreach (var ar in architecturalOption)
        {
            if (ar.Name.Equals(e.Value.ToString()))
            {
                SelectedArchitecturalModel = ar.Name;
            }
        }
        //StateHasChanged();
    }
}