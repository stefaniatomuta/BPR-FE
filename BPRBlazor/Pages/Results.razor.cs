using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Pages;

public partial class Results : ComponentBase
{
    private List<ResultViewModel> _results = new();

    protected override async Task OnInitializedAsync()
    {
        var resultModel = await ResultService.GetAllResultsAsync();
        _results = Mapper.Map<List<ResultViewModel>>(resultModel);
    }
    
    private async Task HandleResultDeletedAsync(Guid id)
    { 
        var confirmed = await JS.InvokeAsync<bool>("handleConfirmation", new object?[]{$"Are you sure you want to delete the result?"});
        if (!confirmed)
        {
            return;
        }
        _results.RemoveAll(result => result.Id == id);
    }
}