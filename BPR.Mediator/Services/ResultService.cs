using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Model.Architectures;
using BPR.Model.Enums;
using BPR.Model.Requests;
using BPR.Model.Results;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class ResultService : IResultService
{
    private readonly IResultRepository _resultRepository;
    private readonly IAnalysisService _analysisService;
    private readonly ILogger<ResultService> _logger;
    private readonly IHttpService _httpService;

    public ResultService(IResultRepository resultRepository, IAnalysisService analysisService, ILogger<ResultService> logger, IHttpService httpService)
    {
        _analysisService = analysisService;
        _resultRepository = resultRepository;
        _logger = logger;
        _httpService = httpService;
    }

    public async Task<IList<AnalysisResult>> GetAllResultsAsync()
    {
        var result = (await _resultRepository.GetAllResultsAsync()).Value;

        if (result == null || !result.Any())
        {
            return new List<AnalysisResult>();
        }

        return result.OrderByDescending(res => res.ResultStart).ToList();
    }

    public async Task<AnalysisResult> GetResultAsync(Guid id)
    {
        var result = (await _resultRepository.GetResultAsync(id)).Value;
        if (result != null)
        {
            return result;
        }

        return new AnalysisResult();
    }


    public async Task<Result> CreateResultAsync(string folderPath, ArchitecturalModel model, List<Rule> rules)
    {
        var resultModel = new AnalysisResult()
        {
            ResultStart = DateTime.UtcNow,
            ResultStatus = ResultStatus.Processing
        };
        var added = await _resultRepository.AddResultAsync(resultModel);

        if (!added.Success)
        {
            return added;
        }

        try
        {
            // TODO - Do something with the results
            var externalAnalysisResults = await HandleExternalAnalysis(folderPath, rules);

            resultModel.Id = added.Value?.Id ?? new Guid();
            resultModel.Violations = await GetViolationsFromAnalysisAsync(folderPath, model, rules);
            resultModel.ResultEnd = DateTime.UtcNow;
            resultModel.ResultStatus = ResultStatus.Finished;
            var addResult = await _resultRepository.UpdateResultAsync(resultModel);
            return addResult.Success ? Result.Ok(addResult) : Result.Fail<Rule>(addResult.Errors, _logger);
        }
        catch (Exception e)
        {
            resultModel.ResultStatus = ResultStatus.Failed;
            resultModel.ResultEnd = DateTime.UtcNow;
            await _resultRepository.UpdateResultAsync(resultModel);
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<Result> DeleteResultAsync(Guid id)
    {
        var deletedModel = await _resultRepository.DeleteResultAsync(id);
        return deletedModel.Success
            ? Result.Ok($"The result was deleted successfully")
            : Result.Fail($"No result found with id: '{id}'", _logger);
    }

    private async Task<List<Violation>> GetViolationsFromAnalysisAsync(string folderPath, ArchitecturalModel model,
        List<Rule> rules)
    {
        var ruleList = rules
            .Select(rule => rule.ViolationType)
            .ToList();

        return await _analysisService.GetAnalysisAsync(folderPath, model, ruleList);
    }

    private async Task<MLAnalysisResponseModel?> HandleExternalAnalysis(string folderPath, List<Rule> rules)
    {
        const string url = "http://localhost:8000";
        const string endpoint = "upload_code";
        var externalRules = rules.ToExternalAnalysisRules();

        if (!externalRules.Any())
        {
            return null;
        }

        var request = new MLAnalysisRequestModel(folderPath, externalRules);
        return await _httpService.PostAsync<MLAnalysisRequestModel, MLAnalysisResponseModel>($"{url}/{endpoint}", request);
    }
}
