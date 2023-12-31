﻿using BPR.Model.Architectures;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPRBlazor.Components.Common;

public partial class SelectArchitectureComponent : ComponentBase
{
    [Parameter, EditorRequired]
    public EventCallback<ArchitectureModel> ArchitectureModelChanged { get; set; }

    [Parameter]
    public Guid? SelectedOption { get; set; }

    private IList<ArchitectureModel> _architectureOptions = new List<ArchitectureModel>();

    protected override async Task OnInitializedAsync()
    {
        _architectureOptions = (await GetArchitectureModelsAsync())
            .OrderBy(option => option.Name)
            .ToList();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var selectedOption = _architectureOptions.FirstOrDefault(option => option.Id == SelectedOption);
            if (selectedOption == null)
            {
                return;
            }

            var optionIndex = _architectureOptions.IndexOf(selectedOption);
            await JS.InvokeVoidAsync("setSelectedElement", new object[] { "selectArchitecture", optionIndex + 1 });
        }
    }

    private Task<IList<ArchitectureModel>> GetArchitectureModelsAsync()
    {
        return ArchitectureModelService.GetArchitectureModelsAsync();
    }

    private async Task OnSelectedValueChangedAsync(ChangeEventArgs e)
    {
        var selectedModelId = Guid.Parse(e.Value?.ToString()!);
        var selectedModel = _architectureOptions.First(option => option.Id == selectedModelId);
        await ArchitectureModelChanged.InvokeAsync(selectedModel);
    }
}