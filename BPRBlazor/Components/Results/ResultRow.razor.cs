using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Results;

public partial class ResultRow : ComponentBase
{
    [Parameter]
    public ResultViewModel Result { get; set; } = default!;
    [Parameter]
    public EventCallback<Guid> ResultDeleted { get; set; }


    private void SeeDetails(Guid id)
    {
        NavigationManager.NavigateTo($"results/{id}");
    }
    
    private async void DeleteAsync(Guid id)
    {
        var result = await ResultService.DeleteResultAsync(id);
        if (result.Success)
        {
            await ResultDeleted.InvokeAsync(id);
        }
    }
}