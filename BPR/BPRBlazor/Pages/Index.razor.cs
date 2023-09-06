namespace BPRBlazor.Pages;

public partial class Index : ComponentBase
{
    private async Task SendDataAsync()
    {
        await HttpService.PostAsync("http://127.0.0.1:8000/post?item=HelloWorld", string.Empty);
    }
}