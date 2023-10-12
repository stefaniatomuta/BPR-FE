using AutoMapper;
using BPR.Mediator.Models;
using BPR.Mediator.Validators;
using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class RuleService : IRuleService
{
    private readonly IRuleRepository _ruleRepository;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly ILogger<RuleService> _logger;
    
    public RuleService(IRuleRepository ruleRepository, IValidatorService validatorService, IMapper mapper, ILogger<RuleService> logger)
    {
        _ruleRepository = ruleRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _logger = logger;
    }

    // TODO - Is this method necessary? We have no user stories related to being able to add new rules.
    // The user will have no access to add new rules to the system, it is used for facilitating BE operations
    public async Task<Result> AddRuleAsync(Rule rule)
    {
        var result = await _validatorService.ValidateRuleAsync(rule);
        if (result.Success)
        {
            var addResult = await _ruleRepository.AddRuleAsync(_mapper.Map<Rule, RuleCollection>(rule));
            return addResult.Success ? Result.Ok(addResult) : Result.Fail<RuleCollection>(addResult.Errors, _logger);
        }
        return result;
    }

    public async Task<IList<Rule>> GetRulesAsync()
    {
        var rules = await _ruleRepository.GetRulesAsync();
        var documents = new List<Rule>();
        foreach (var doc in rules)
        {
            documents.Add(_mapper.Map<RuleCollection, Rule>(doc));
        }
        return documents;
    }
}