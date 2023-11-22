using BPR.Mediator.Utils;
using BPR.Model.Architectures;

namespace BPR.Mediator.Interfaces;

public interface IRuleService
{
    public Task<Result> AddRuleAsync(Rule rule);
    public Task<IList<Rule>> GetRulesAsync();
}