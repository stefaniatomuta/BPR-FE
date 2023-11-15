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
    
    private void HandleResultDeleted(Guid id)
    {
        _results.RemoveAll(result => result.Id == id);
    }
}