using BPRBE.Models.Persistence;
using BPRBlazor.Components.Common;
using Microsoft.AspNetCore.Components;

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
        try
        {
            var result = await service.DeleteArchitectureModelAsync(_selectedModel!.Id);

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
