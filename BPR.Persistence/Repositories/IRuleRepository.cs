using BPR.Persistence.Models;
using BPR.Persistence.Utils;

namespace BPR.Persistence.Repositories;

public interface IRuleRepository
{
    Task<Result> AddRuleAsync(RuleCollection ruleCollection);
    Task<IList<RuleCollection>> GetRulesAsync();
}