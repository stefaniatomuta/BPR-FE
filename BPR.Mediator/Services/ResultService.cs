using AutoMapper;
using BPR.Analysis.Mappers;
using BPR.Analysis.Models;
using BPR.Analysis.Services;
using BPR.Mediator.Enums;
using BPR.Mediator.Models;
using BPR.Mediator.Validators;
using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class ResultService : IResultService
{
    private readonly IResultRepository _resultRepository;
    private readonly IAnalysisService _analysisService;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly ILogger<ResultService> _logger;
    
    public ResultService(IResultRepository resultRepository, IAnalysisService analysisService, IValidatorService validatorService, IMapper mapper, ILogger<ResultService> logger)
    {
        _analysisService = analysisService;
        _resultRepository = resultRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<IList<ResultModel>> GetAllResultsAsync()
    {
        var result = (await _resultRepository.GetAllResultsAsync()).Val;

        if (result == null || !result.Any())
        {
            return new List<ResultModel>();
        }
        return result
            .Select(res => _mapper.Map<ResultModel>(res))
            .OrderByDescending(res => res.ResultStart)
            .ToList();
    }

    public async Task<ResultModel> GetResultAsync(Guid id)
    {
        var result = (await _resultRepository.GetResultAsync(id)).Val;
        if (result != null)
        {
            var resultModel = _mapper.Map<ResultModel>(result);
            return resultModel;
        }

        return new ResultModel();
    }


    public async Task<Result> CreateResultAsync(string folderPath, ArchitecturalModel model, List<Rule> rules)
    {
        var resultModel = new ResultModel()
        {
            ResultStart = DateTime.Now,
            ResultStatus = (int)ResultStatus.Processing
        };
        var added = await _resultRepository.AddResultAsync(_mapper.Map<ResultCollection>(resultModel));

        if (!added.Success)
        {
            return added;
        }

        try
        {
            resultModel.Id = added.Val?.Id ?? default;
            resultModel.Violations = await GetViolationsFromAnalysisAsync(folderPath, model, rules);
            var result = await _validatorService.ValidateResultAsync(resultModel);
            if (result.Success)
            {
                resultModel.ResultEnd = DateTime.Now;
                resultModel.ResultStatus = ResultStatus.Finished;
                var addResult = await _resultRepository.UpdateResultAsync(_mapper.Map<ResultCollection>(resultModel));
                return addResult.Success ? Result.Ok(addResult) : Result.Fail<RuleCollection>(addResult.Errors, _logger);
            }
            return result;
        }
        catch (Exception e)
        {
            resultModel.ResultStatus = ResultStatus.Failed;
            await _resultRepository.UpdateResultAsync(_mapper.Map<ResultCollection>(resultModel));
            Console.WriteLine(e);
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

    private async Task<List<ViolationModel>> GetViolationsFromAnalysisAsync(string folderPath, ArchitecturalModel model, List<Rule> rules)
    {
        var ruleList = rules
            .Select(rule => AnalysisRuleMapper.GetAnalysisRuleEnum(rule.Name))
            .ToList();
        var architecturalModel = _mapper.Map<AnalysisArchitecturalModel>(model);
   
        var violations = await _analysisService.GetAnalysisAsync(folderPath, architecturalModel, ruleList);
        return _mapper.Map<List<ViolationModel>>(violations);
    }
}