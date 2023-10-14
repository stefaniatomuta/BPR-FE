using BPR.Persistence.Models;
using BPR.Mediator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MongoDB.Bson;

namespace BPRBlazor.Components.Common;

public partial class SelectArchitectureComponent : ComponentBase
{
    [Parameter]
    public EventCallback<ArchitecturalModel> ArchitectureModelChanged { get; set; }

    private IList<ArchitecturalModel> _architectureOptions = new List<ArchitecturalModel>();

    protected override async Task OnInitializedAsync()
    {
        _architectureOptions = await GetArchitecturalModels();
    }

    private async Task<IList<ArchitecturalModel>> GetArchitecturalModels()
    {
        return await DependencyService.GetArchitecturalModelsAsync();
    }

    private async Task OnSelectedValueChanged(ChangeEventArgs e)
    {
        //TODO:
        var selectedModelId = Guid.Parse(e.Value?.ToString()!);
        var selectedModel = _architectureOptions.First(option => option.Id == selectedModelId);
        await ArchitectureModelChanged.InvokeAsync(selectedModel);
    }

    public async Task RemoveOptionById(Guid modelId)
    {
        var model = _architectureOptions.First(option => option.Id == modelId);
        _architectureOptions.Remove(model);
        await JS.InvokeVoidAsync("removeSelectedElement", "selectArchitecture");
    }
}