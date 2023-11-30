using BPR.Model.Api;
using MudBlazor;

namespace BPRBlazor.Shared;

public partial class MainLayout
{
    protected override void OnInitialized()
    {
        MessageConsumerService.MessageReceivedEvent += OnMessageReceivedAsync;
    }

    private async Task OnMessageReceivedAsync(MLAnalysisResponseModel response)
    {
        await InvokeAsync(() =>
        {
            // TODO - do something with the response. Save in DB and notify user?
            StateHasChanged();
        });
    }

    private void ShowSnackbar()
    {
        string message = "Analysis results completed! Click to view results";
        Snackbar.Add(message, Severity.Success, config =>
        {
            config.HideIcon = true;
            config.Onclick = (snackbar) =>
            {
                NavigationManager.NavigateTo("/results");
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
