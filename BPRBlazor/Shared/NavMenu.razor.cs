using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Shared;

public partial class NavMenu : ComponentBase
{
    private bool _collapseNavMenu = true;

    private string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }
}