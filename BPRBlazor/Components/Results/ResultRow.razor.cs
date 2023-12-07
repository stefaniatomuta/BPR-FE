using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results;

public partial class ResultRow : ComponentBase
{
    [Parameter, EditorRequired]
    public ResultViewModel Result { get; set; } = default!;

    [Parameter, EditorRequired]
    public EventCallback<Guid> ResultDeleted { get; set; }

    private void SeeDetails(Guid id)
    {
        NavigationManager.NavigateTo($"results/{id}");
    }
    
    private async void DeleteAsync(Guid id)
    {
        var confirmed = await JS.InvokeAsync<bool>("handleConfirmation", new object?[]{$"Are you sure you want to delete the result?"});
        if (!confirmed)
        {
            return;
        }
        var result = await ResultService.DeleteResultAsync(id);
        if (result.Success)
        {
            await ResultDeleted.InvokeAsync(id);
        }
    }
}