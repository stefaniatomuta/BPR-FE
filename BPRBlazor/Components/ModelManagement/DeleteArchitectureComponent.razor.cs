using BPR.Persistence.Models;
using BPRBlazor.Components.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPRBlazor.Components.ModelManagement;

public partial class DeleteArchitectureComponent : ComponentBase
{
    private ArchitecturalModelCollection? _selectedModel;
    private string _resultMessage = string.Empty;
    private string _resultCssClass = string.Empty;

    private SelectArchitectureComponent? selectArchitectureComponent;

    private void HandleModelChange(ArchitecturalModelCollection modelCollection)
    {
        _selectedModel = modelCollection;
    }

    private async Task DeleteSelectedModel()
    {
        var confirmed = await JS.InvokeAsync<bool>("handleConfirmation", $"Are you sure you want to delete the '{_selectedModel!.Name}' modelCollection?");
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
                    var id = Guid.Parse(_selectedModel.Id.ToString());
                    await selectArchitectureComponent.RemoveOptionById(id);
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
