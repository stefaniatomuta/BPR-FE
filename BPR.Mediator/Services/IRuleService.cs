using BPR.Mediator.Models;
using BPR.Persistence.Utils;

namespace BPR.Mediator.Services;

public interface IRuleService
{
    public Task<Result> AddRuleAsync(Rule rule);
    public Task<IList<Rule>> GetRulesAsync();
}