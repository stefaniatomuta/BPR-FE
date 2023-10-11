using BPRBE.Models.Persistence;
using BPRBlazor.Components.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPRBlazor.Components.ModelManagement;

public partial class DeleteArchitectureComponent : ComponentBase
{
    private ArchitecturalModel? _selectedModel;
    private string _resultMessage = string.Empty;
    private string _resultCssClass = string.Empty;

    private SelectArchitectureComponent? selectArchitectureComponent;

    private void HandleModelChange(ArchitecturalModel model)
    {
        _selectedModel = model;
    }

    private async Task DeleteSelectedModel()
    {
        var confirmed = await JS.InvokeAsync<bool>("handleConfirmation", $"Are you sure you want to delete the '{_selectedModel!.Name}' model?");
        if (!confirmed)
        {
            return;
        }

        try
        {
            var result = await service.DeleteArchitectureModelAsync(_selectedModel.Id);

            if (result.Success)
            {
                if (selectArchitectureComponent is not null)
                {
                    await selectArchitectureComponent.RemoveOptionById(_selectedModel.Id);
                }

                _resultMessage = "Model successfully deleted!";
                _resultCssClass = "success";
                _selectedModel = null;
            }
            else
            {
                _resultMessage = result.Errors.First();
                _resultCssClass = "error";
            }
        }
        catch (Exception ex)
        {
            _resultMessage = ex.Message;
            _resultCssClass = "error";
        }
    }
}
