using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Mediator.Validators;
using BPR.Model.Rules;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class RuleService : IRuleService
{
    private readonly IRuleRepository _ruleRepository;
    private readonly IValidatorService _validatorService;
    private readonly ILogger<RuleService> _logger;

    public RuleService(IRuleRepository ruleRepository, IValidatorService validatorService, ILogger<RuleService> logger)
    {
        _ruleRepository = ruleRepository;
        _validatorService = validatorService;
        _logger = logger;
    }

    // The user will have no access to add new rules to the system, it is used for facilitating BE operations
    public async Task<Result> AddRuleAsync(Rule rule)
    {
        var result = await _validatorService.ValidateRuleAsync(rule);
        if (result.Success)
        {
            var addResult = await _ruleRepository.AddRuleAsync(rule);
            return addResult.Success ? Result.Ok(addResult) : Result.Fail<Rule>(addResult.Errors, _logger);
        }

        return result;
    }

    public async Task<IList<Rule>> GetRulesAsync()
    {
        return await _ruleRepository.GetRulesAsync();
    }
}