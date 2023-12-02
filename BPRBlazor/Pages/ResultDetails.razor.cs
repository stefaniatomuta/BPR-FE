using BPR.Model.Results;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPRBlazor.Pages;

public partial class ResultDetails : ComponentBase
{
    [Parameter] 
    public Guid Id { get; set; }

    private ResultViewModel? _result;
    private List<ViolationViewModel> _filteredViolations = new();
    
    protected override async Task OnAfterRenderAsync(bool firstRender) 
    {
        if (firstRender)
        {
            var resultModel = await ResultService.GetResultAsync(Id);
            if (resultModel != null)
            {
                _result = Mapper.Map<AnalysisResult, ResultViewModel>(resultModel);
                if (resultModel.ExtendedAnalysisResults != null)
                {
                    _result.Violations.AddRange(resultModel.ExtendedAnalysisResults.MapToViolations()
                        .Select(violation => Mapper.Map<ViolationViewModel>(violation)));
                }

                _filteredViolations = new List<ViolationViewModel>(_result.Violations);
                StateHasChanged();
            }
        }
    } 
    
    private void HandleViolationType(ViolationTypeViewModel value)
    {
        if (value.IsChecked && _result != null)
        {
            _filteredViolations.AddRange(_result.Violations.Where(violation => violation.Type == value.ViolationType));
        }
        else
        {
            _filteredViolations.RemoveAll(violation => violation.Type == value.ViolationType);
        }
    }

    private List<ViolationTypeViewModel> GetCurrentViolationTypes()
    {
        if (_result == null || !_result.Violations.Any())
        {
            return new List<ViolationTypeViewModel>();
        }
        
        return _result.Violations.Select(violation => violation.Type)
            .Distinct()
            .Select(violation => new ViolationTypeViewModel(violation))
            .ToList();
    }
    
    private async Task DownloadPdf()
    {
        await JsRuntime.InvokeVoidAsync("DownloadResultsToPDF");
    }
}