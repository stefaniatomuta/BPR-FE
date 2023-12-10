using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BPRBlazor.Services;

public class ToastService
{
    private readonly ISnackbar _snackbar;
    private readonly NavigationManager _navigationManager;

    public ToastService(ISnackbar snackbar, NavigationManager navigationManager)
    {
        _snackbar = snackbar;
        _navigationManager = navigationManager;
    }

    public void ShowSnackbar(Guid resultId)
    {
        string message = "Analysis complete! Click to view results";
        _snackbar.Add(message, Severity.Success, config =>
        {
            config.HideIcon = true;
            config.Onclick = (_) =>
            {
                _navigationManager.NavigateTo($"/results/{resultId}");
                return Task.CompletedTask;
            };
        });
    }
}
