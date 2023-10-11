using BPRBE.Models;
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
        _resultModel = new ResultModel();
        _resultModel.Violations = StateContainer.Property;
    }

    public void Dispose() {
        StateContainer.OnChange -= StateHasChanged;
    }
    
    private async Task DownloadPdf()
    {
        await JsRuntime.InvokeVoidAsync("DownloadResultsToPDF");
    }
}