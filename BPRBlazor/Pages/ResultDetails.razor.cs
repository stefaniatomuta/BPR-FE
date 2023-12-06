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
    private Dictionary<string, double>? _codeLinesMetrics;
    private Dictionary<string, int>? _externalApiCalls;
    private Dictionary<string, int>? _classCouplings;
    private Dictionary<string, int>? _codeLinesPerFile;
    private Dictionary<string, int>? _commentLinesPerFile;
    private Dictionary<string, Dictionary<string, double>>? _codeSimilarities;

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
        _codeLinesPerFile = ExtendedAnalysisHandler.HandleCodeLinesPerFile(results);
        _commentLinesPerFile = ExtendedAnalysisHandler.HandleCommentLinesPerFile(results);
        _codeLinesMetrics = ExtendedAnalysisHandler.HandleCodeLinesMetrics(results);
        _externalApiCalls = ExtendedAnalysisHandler.HandleExternalApiCalls(results);
        _classCouplings = ExtendedAnalysisHandler.HandleClassCoupling(results);
        _codeSimilarities = ExtendedAnalysisHandler.HandleCodeSimilarities(results);
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
        if (_result == null)
        {
            return new List<ViolationTypeViewModel>();
        }
        
        return _result.ViolationTypes
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