using BPRBE.Models.Persistence;

namespace BPRBE.Persistence;

public interface IRuleRepository
{
    public Task<Result> AddRuleAsync(Rule rule);
    public Task<IList<Rule>> GetRulesAsync();
}