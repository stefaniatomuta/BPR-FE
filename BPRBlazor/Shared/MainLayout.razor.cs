using BPR.Model.Requests;

namespace BPRBlazor.Shared;

public partial class MainLayout
{
    private string response = string.Empty;

    protected override void OnInitialized()
    {
        MessageConsumerService.MessageReceivedEvent += OnMessageReceivedAsync;
    }

    private async Task OnMessageReceivedAsync(MLAnalysisResponseModel response)
    {
        await InvokeAsync(() =>
        {
            // TODO - do something with the response. Save in DB and notify user?
            this.response = response.ToString()!;
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        MessageConsumerService.MessageReceivedEvent -= OnMessageReceivedAsync;
    }
}
