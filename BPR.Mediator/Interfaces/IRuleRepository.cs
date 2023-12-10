using BPR.Mediator.Utils;
using BPR.Model.Rules;

namespace BPR.Mediator.Interfaces;

public interface IRuleRepository
{
    Task<Result> AddRuleAsync(Rule rule);
    Task<IList<Rule>> GetRulesAsync();
}