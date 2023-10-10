using BPRBE.Models;
using Microsoft.AspNetCore.Components;

namespace BPRBlazor.Pages;

public partial class Results : ComponentBase
{
    private ResultModel? _resultModel;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        StateContainer.OnChange += StateHasChanged;
        _resultModel = new ResultModel();
        _resultModel.Violations = StateContainer.Property;
    }

    public void Dispose() {
        StateContainer.OnChange -= StateHasChanged;
    }
}