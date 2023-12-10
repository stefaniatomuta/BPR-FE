using BPR.Mediator.Utils;
using BPR.Model.Rules;

namespace BPR.Mediator.Interfaces;

public interface IRuleService
{
    public Task<Result> AddRuleAsync(Rule rule);
    public Task<IList<Rule>> GetRulesAsync();
}