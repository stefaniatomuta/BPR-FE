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
        await ResultService.UpdateAndFinishResultAsync(response.CorrelationId, response);
        ToastService.ShowSnackbar(response.CorrelationId);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        MessageConsumerService.MessageReceivedEvent -= OnMessageReceivedAsync;
    }
}
