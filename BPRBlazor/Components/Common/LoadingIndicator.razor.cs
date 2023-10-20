using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Components.Common;

public partial class LoadingIndicator : ComponentBase
{
    public bool IsLoading { get; private set; }

    public void ToggleLoading()
    {
        IsLoading = !IsLoading;
        StateHasChanged();
    }
}
