using BPR.Model.Enums;
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
    private List<ViolationTypeViewModel> _violationTypes = new();
    private List<ViolationViewModel> _filteredViolations = new();

    private Dictionary<string, double>? _conditionalFrequencies;
    private Dictionary<string, int>? _solutionMetrics;
    
    protected override async Task OnAfterRenderAsync(bool firstRender) 
    {
        if (firstRender)
        {
            var resultModel = await ResultService.GetResultAsync(Id);
            if (resultModel != null)
            {
                _result = Mapper.Map<AnalysisResult, ResultViewModel>(resultModel);
                _filteredViolations = new List<ViolationViewModel>(_result.Violations);
                HandleExtendedAnalysisResults(_result.ExtendedAnalysisResults);
                _violationTypes = GetCurrentViolationTypes();
                StateHasChanged();
            }
        }
    }

    private void HandleExtendedAnalysisResults(ExtendedAnalysisResults? results)
    {
        if (results == null)
        {
            return;
        }

        _conditionalFrequencies = ExtendedAnalysisHandler.HandleConditionalStatements(results);
        _solutionMetrics = ExtendedAnalysisHandler.HandleSolutionMetrics(results);
    }

    private void HandleViolationType(ViolationTypeViewModel value)
    {
        _violationTypes.Single(type => type.ViolationType == value.ViolationType).IsChecked = value.IsChecked;
        if (value.ViolationType != ViolationType.ForbiddenDependency &&
            value.ViolationType != ViolationType.MismatchedNamespace)
        {
            return;
        }
        if (value.IsChecked && _result != null)
        {
            _filteredViolations.AddRange(
                _result.Violations.Where(violation => violation.Type == value.ViolationType));
        }
        else
        {
            _filteredViolations.RemoveAll(violation => violation.Type == value.ViolationType);
        }
    }

    private List<ViolationTypeViewModel> GetCurrentViolationTypes()
    { 
        if (_result == null || (!_result.Violations.Any() && _result.ExtendedAnalysisResults == null))
        {
            return new List<ViolationTypeViewModel>();
        }
        var violationTypes = new List<ViolationType>();
        violationTypes.AddRange(_result.Violations.Select(violation => violation.Type));

        if (_conditionalFrequencies != null)
        {
            violationTypes.Add(ViolationType.ConditionalStatements);
        }

        if (_solutionMetrics != null)
        {
            violationTypes.Add(ViolationType.SolutionMetrics);
        }
        
        if (_result.ExtendedAnalysisResults?.ExternalApiCalls != null &&
            _result.ExtendedAnalysisResults.ExternalApiCalls.Any())
        {
            violationTypes.Add(ViolationType.ExternalCalls);
        }
        
        if (_result.ExtendedAnalysisResults?.EndOfLifeFrameworks != null &&
            _result.ExtendedAnalysisResults.EndOfLifeFrameworks.Any())
        {
            violationTypes.Add(ViolationType.ExternalCalls);
        }

        return violationTypes
            .Distinct()
            .Select(type => new ViolationTypeViewModel(type))
            .OrderBy(viewModel => viewModel.Name)
            .ToList();
    }
    
    private async Task DownloadPdf()
    {
        await JsRuntime.InvokeVoidAsync("DownloadResultsToPDF");
    }
}