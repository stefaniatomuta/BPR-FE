using BPR.Mediator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPRBlazor.Pages;

public partial class Results : ComponentBase
{
    private ResultModel? _resultModel;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        StateContainer.OnChange += StateHasChanged;
        _resultModel = new ResultModel
        {
            Violations = StateContainer.Property
        };
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        StateContainer.OnChange -= StateHasChanged;
    }
    
    private async Task DownloadPdf()
    {
        await JsRuntime.InvokeVoidAsync("DownloadResultsToPDF");
    }
}