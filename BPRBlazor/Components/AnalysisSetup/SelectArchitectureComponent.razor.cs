using BPRBE.Models.Persistence;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.AnalysisSetup;

public partial class SelectArchitectureComponent : ComponentBase
{
    [Parameter]
    public string SelectedArchitecturalModel { get; set; } = default!;

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
        SelectedArchitecturalModel = e.Value.ToString();
        Console.WriteLine("It is definitely: " + SelectedArchitecturalModel);
        StateHasChanged();
    }
}