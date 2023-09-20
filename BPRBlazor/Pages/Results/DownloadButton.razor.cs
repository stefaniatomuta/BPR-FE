namespace BPRBlazor.Pages.Results;

public partial class DownloadButton : ComponentBase
{
    private async Task DownloadPdf()
    {
        await JsRuntime.InvokeVoidAsync("DownloadResultsToPDF");
    }
}