using BPR.Mediator.Models;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPRBlazor.Pages;

public partial class Results : ComponentBase
{
    private ResultModel? _resultModel;
    private List<ViolationModel> _filteredViolations = new();
    
    protected override async Task OnAfterRenderAsync (bool firstRender) 
    {
        if (firstRender)
        {
            _resultModel = new ResultModel
            {
                Violations = (await ProtectedLocalStore.GetAsync<List<ViolationModel>>("violations")).Value ?? new()
            };
            _filteredViolations = new List<ViolationModel>(_resultModel.Violations);
            StateHasChanged();
        }
    } 
    
    private void HandleViolationType(ViolationTypeViewModel value)
    {
        if (value.IsChecked && _resultModel != null)
        {
            _filteredViolations.AddRange(_resultModel.Violations.Where(violation => violation.Type == value.ViolationType));
        }
        else
        {
            _filteredViolations.RemoveAll(violation => violation.Type == value.ViolationType);
        }
    }

    private List<ViolationTypeViewModel> GetCurrentViolationTypes()
    {
        if (_resultModel == null || !_resultModel.Violations.Any())
        {
            return new List<ViolationTypeViewModel>();
        }
        var violationTypes = _resultModel.Violations.Select(violation => violation.Type).ToHashSet();
        return violationTypes.Select(violation => new ViolationTypeViewModel(violation)).ToList();
    }
    
    private async Task DownloadPdf()
    {
        await JsRuntime.InvokeVoidAsync("DownloadResultsToPDF");
    }
}