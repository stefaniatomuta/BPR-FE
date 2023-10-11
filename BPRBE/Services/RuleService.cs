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

    // TODO - Is this method necessary? We have no user stories related to being able to add new rules.
    public async Task<Result> AddRuleAsync(Rule rule)
    {
        var result = await _validatorService.ValidateRuleAsync(rule);
        if (result.Success)
        {
            return await _ruleRepository.AddRuleAsync(rule);
        }
        return result;
    }

    public async Task<IList<Rule>> GetRulesAsync()
    {
        return await _ruleRepository.GetRulesAsync();
    }
}