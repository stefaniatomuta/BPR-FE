using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Validators;

namespace BPRBE.Services;

public class RuleService : IRuleService
{
    private readonly IRuleRepository _ruleRepository;
    private readonly IValidatorService _validatorService;

    public RuleService(IRuleRepository ruleRepository, IValidatorService validatorService)
    {
        _ruleRepository = ruleRepository;
        _validatorService = validatorService;
    }

    public async Task<Result> AddRuleAsync(Rule rule)
    {
        var result = _validatorService.ValidateRuleAsync(rule);
        return await _ruleRepository.AddRuleAsync(rule);
    }

    public async Task<IList<Rule>> GetRulesAsync()
    {
        return await _ruleRepository.GetRulesAsync();
    }
}