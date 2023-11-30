using BPR.Model.Api;
using MudBlazor;

namespace BPRBlazor.Shared;

public partial class MainLayout
{
    protected override void OnInitialized()
    {
        MessageConsumerService.MessageReceivedEvent += OnMessageReceivedAsync;
    }

    private Task OnMessageReceivedAsync(MLAnalysisResponseModel response)
    {
        ShowSnackbar(response.CorrelationId);
        return Task.CompletedTask;
    }

    private void ShowSnackbar(Guid resultId)
    {
        string message = "Analysis results completed! Click to view results";
        Snackbar.Add(message, Severity.Success, config =>
        {
            config.HideIcon = true;
            config.Onclick = (snackbar) =>
            {
                NavigationManager.NavigateTo($"/results/{resultId}");
                return Task.CompletedTask;
            };
        });
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        MessageConsumerService.MessageReceivedEvent -= OnMessageReceivedAsync;
    }
}
