using BPRBE.Models.Persistence;

namespace BPRBE.Persistence;

public interface IRuleRepository
{
    Task<Result> AddRuleAsync(Rule rule);
    Task<IList<Rule>> GetRulesAsync();
}