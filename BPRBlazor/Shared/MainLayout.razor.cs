using BPR.Model.Results;

namespace BPRBlazor.Shared;

public partial class MainLayout
{
    protected override void OnInitialized()
    {
        MessageConsumerService.MessageReceivedEvent += OnMessageReceivedAsync;
    }

    private async Task OnMessageReceivedAsync(ExtendedAnalysisResults response)
    {
        var result = await ResultService.UpdateAndFinishResultAsync(response.CorrelationId, response);
        if (result.Success)
        {
            ToastService.ShowSnackbar(response.CorrelationId);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        MessageConsumerService.MessageReceivedEvent -= OnMessageReceivedAsync;
    }
}
