using BPR.Model.Enums;
using BPR.Model.Results;
using BPR.Model.Results.External;
using BPRBlazor.Components.Common;
using BPRBlazor.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPRBlazor.Pages;

public partial class ResultDetails : ComponentBase
{
    [Parameter] 
    public Guid Id { get; set; }

    private ResultViewModel? _result;
    private List<RuleTypeViewModel> _ruleTypes = new();
    private List<ViolationViewModel> _filteredViolations = new();
    private LoadingIndicator? _loadingIndicator;

    private string? _technicalDebtClassification;
    private Dictionary<string, double>? _conditionalFrequencies;
    private Dictionary<string, int>? _solutionMetrics;
    private Dictionary<string, double>? _codeLinesMetrics;
    private Dictionary<string, ExternalApiMetrics>? _externalApiCalls;
    private Dictionary<string, int>? _classCouplings;
    private Dictionary<string, int>? _codeLinesPerFile;
    private Dictionary<string, int>? _commentLinesPerFile;
    private Dictionary<string, Dictionary<string, double>>? _codeSimilarities;

    protected override async Task OnAfterRenderAsync(bool firstRender) 
    {
        if (firstRender)
        {
            _loadingIndicator?.ToggleLoading(true);
            var resultModel = await ResultService.GetResultAsync(Id);
            if (resultModel != null)
            {
                _result = Mapper.Map<AnalysisResult, ResultViewModel>(resultModel);
                _filteredViolations = new List<ViolationViewModel>(_result.Violations);
                HandleExtendedAnalysisResults(_result.ExtendedAnalysisResults);
                _ruleTypes = GetCurrentRuleTypes();
                StateHasChanged();
            }
            _loadingIndicator?.ToggleLoading(false);
        }
    }

    private void HandleExtendedAnalysisResults(ExtendedAnalysisResults? results)
    {
        if (results == null)
        {
            return;
        }

        _technicalDebtClassification = ExtendedAnalysisHandler.HandleTechnicalDebtClassification(results);
        _conditionalFrequencies = ExtendedAnalysisHandler.HandleConditionalStatements(results);
        _solutionMetrics = ExtendedAnalysisHandler.HandleSolutionMetrics(results);
        _codeLinesPerFile = ExtendedAnalysisHandler.HandleCodeLinesPerFile(results);
        _commentLinesPerFile = ExtendedAnalysisHandler.HandleCommentLinesPerFile(results);
        _codeLinesMetrics = ExtendedAnalysisHandler.HandleCodeLinesMetrics(results);
        _externalApiCalls = ExtendedAnalysisHandler.HandleExternalApiCalls(results);
        _classCouplings = ExtendedAnalysisHandler.HandleClassCoupling(results);
        _codeSimilarities = ExtendedAnalysisHandler.HandleCodeSimilarities(results);
    }

    private void HandleRuleType(RuleTypeViewModel value)
    {
        _ruleTypes.Single(type => type.RuleType == value.RuleType).IsChecked = value.IsChecked;
        if (value.RuleType != RuleType.ForbiddenDependency &&
            value.RuleType != RuleType.MismatchedNamespace)
        {
            return;
        }
        if (value.IsChecked && _result != null)
        {
            _filteredViolations.AddRange(
                _result.Violations.Where(violation => violation.Type == value.RuleType));
        }
        else
        {
            _filteredViolations.RemoveAll(violation => violation.Type == value.RuleType);
        }
    }

    private List<RuleTypeViewModel> GetCurrentRuleTypes()
    {
        if (_result == null)
        {
            return new List<RuleTypeViewModel>();
        }
        
        return _result.RuleTypes
            .Distinct()
            .Select(type => new RuleTypeViewModel(type))
            .OrderBy(viewModel => viewModel.Name)
            .ToList();
    }
    
    private async Task DownloadPdf()
    {
        _loadingIndicator?.ToggleLoading(true);
        await JsRuntime.InvokeVoidAsync("TransformToPng");
        _loadingIndicator?.ToggleLoading(false);
        await JsRuntime.InvokeVoidAsync("DownloadResultsToPDF");
    }

    private bool ShouldDisplayResults(RuleType type)
    {
        return _ruleTypes.SingleOrDefault(t => t.RuleType == type)?.IsChecked ?? false;
    }
}