namespace BPRBlazor.Pages.Results;

public partial class DownloadButton : ComponentBase
{
    private const string FileName = "Download";
    
    private async Task DownloadPdf()
    {
        var html = new ComponentRenderer<ResultDetails>().Render();
        await JsRuntime.InvokeVoidAsync("exportHTMLtoPDF", html, FileName);
    }
}