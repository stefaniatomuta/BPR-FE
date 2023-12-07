using BPR.Mediator.Interfaces;
using BPR.Mediator.Interfaces.Messaging;
using BPR.Mediator.Utils;
using BPR.Model.Architectures;
using BPR.Model.Enums;
using BPR.Model.Results;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class ResultService : IResultService
{
    private readonly IResultRepository _resultRepository;
    private readonly IAnalysisService _analysisService;
    private readonly ILogger<ResultService> _logger;
    private readonly ISender _messagingService;

    public ResultService(IResultRepository resultRepository, IAnalysisService analysisService, ILogger<ResultService> logger, ISender messagingService)
    {
        _analysisService = analysisService;
        _resultRepository = resultRepository;
        _logger = logger;
        _messagingService = messagingService;
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

    public async Task<AnalysisResult?> GetResultAsync(Guid id)
    {
        var result = (await _resultRepository.GetResultAsync(id)).Value;
        return result;
    }


    public async Task<Result<AnalysisResult>> CreateResultAsync(string folderPath, ArchitecturalModel model, List<Rule> rules, string analysisTitle)
    {
        var resultModel = new AnalysisResult(analysisTitle)
        {
            ResultStart = DateTime.UtcNow,
            ResultStatus = ResultStatus.Processing
        };

        try
        {
            var folderId = GetGuidFromFolderPath(folderPath);
            resultModel.Id = folderId;
            
            var added = await _resultRepository.AddResultAsync(resultModel);
            if (!added.Success)
            {
                return added;
            }
        
            resultModel.Violations = await GetViolationsFromAnalysisAsync(folderPath, model, rules);
            resultModel.ArchitecturalModel = model;
            resultModel.ViolationTypes = rules.Select(rule => rule.ViolationType).ToList();

            if (!await HandleExternalAnalysis(folderPath, rules, resultModel.Id))
            {
                resultModel.ResultStatus = ResultStatus.Finished;
                resultModel.ResultEnd = DateTime.UtcNow;
            }

            var addResult = await _resultRepository.UpdateResultAsync(resultModel);

            return addResult.Success 
                ? Result.Ok(addResult).Value!
                : Result.Fail<AnalysisResult>(addResult.Errors, _logger);
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

    public async Task<Result> UpdateAndFinishResultAsync(Guid id, ExtendedAnalysisResults result)
    {
        var model = await GetResultAsync(id);

        if (model == null)
        {
            return Result.Fail($"No result found with id: '{id}'", _logger);
        }

        model.ExtendedAnalysisResults = result;
        model.ResultEnd = DateTime.UtcNow;
        model.ResultStatus = ResultStatus.Finished;
        await _resultRepository.UpdateResultAsync(model);

        return Result.Ok(model);
    }

    public async Task<Result> DeleteResultAsync(Guid id)
    {
        var deletedModel = await _resultRepository.DeleteResultAsync(id);
        return deletedModel.Success
            ? Result.Ok($"The result was deleted successfully")
            : Result.Fail($"No result found with id: '{id}'", _logger);
    }

    private async Task<List<Violation>> GetViolationsFromAnalysisAsync(string folderPath, ArchitecturalModel model, List<Rule> rules)
    {
        var ruleList = rules
            .Select(rule => rule.ViolationType)
            .ToList();

        return await _analysisService.GetAnalysisAsync(folderPath, model, ruleList);
    }

    private async Task<bool> HandleExternalAnalysis(string folderPath, List<Rule> rules, Guid correlationId)
    {
        var externalRules = rules.ToExternalAnalysisRules();

        if (!externalRules.Any())
        {
            return false;
        }

        await _messagingService.SendAsync(folderPath, externalRules, correlationId);
        return true;
    }

    private Guid GetGuidFromFolderPath(string folderPath)
    {
        foreach (var folderName in folderPath.Split("\\"))
        {
            if (Guid.TryParse(folderName, out var guid))
            {
                return guid;
            }
        }

        throw new Exception("No guid found on selected path");
    }
}
