using BPRBE.Models.Persistence;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class SelectArchitectureComponent : ComponentBase
{
    [Parameter] 
    public string? SelectedArchitecturalModel { get; set; }
    [Parameter]
    public EventCallback<string> ChildParameterChanged { get; set; }

    public IList<ArchitecturalModel> architecturalOption = new List<ArchitecturalModel>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        architecturalOption = await GetArchitecturalModels();
    }

    private async Task<IList<ArchitecturalModel>> GetArchitecturalModels()
    {
        return await DependencyService.GetArchitecturalModelsAsync();
    }
    public async Task OnSelectedValueChanged(ChangeEventArgs e)
    {
        foreach (var ar in architecturalOption)
        {
            if (ar.Name.Equals(e.Value))
            {
                SelectedArchitecturalModel = ar.Name;
                await ChildParameterChanged.InvokeAsync(SelectedArchitecturalModel);
            }
        }
    }
}