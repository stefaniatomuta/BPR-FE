using BPRBE.Models.Persistence;

namespace BPRBE.Services;

public interface IRuleService
{
    public Task<Result> AddRuleAsync(Rule rule);
    public Task<IList<Rule>> GetRulesAsync();

}