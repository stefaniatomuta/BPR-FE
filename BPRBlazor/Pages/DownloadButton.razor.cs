namespace BPRBlazor.Pages;

public partial class DownloadButton : ComponentBase
{
    private async Task DownloadPdf()
    {
        var html = new ComponentRenderer<Results>().Render();
        const string fileName = "Download";
        await JsRuntime.InvokeVoidAsync("exportHTMLtoPDF", html, fileName);
    }
}