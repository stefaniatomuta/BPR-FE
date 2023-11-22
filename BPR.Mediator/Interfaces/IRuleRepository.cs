using BPR.Mediator.Utils;
using BPR.Model.Architectures;

namespace BPR.Mediator.Interfaces;

public interface IRuleRepository
{
    Task<Result> AddRuleAsync(Rule ruleCollection);
    Task<IList<Rule>> GetRulesAsync();
}