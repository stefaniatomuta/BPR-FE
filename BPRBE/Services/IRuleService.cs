using BPR.Persistence.Utils;
using BPRBE.Models;

namespace BPRBE.Services;

public interface IRuleService
{
    public Task<Result> AddRuleAsync(Rule rule);
    public Task<IList<Rule>> GetRulesAsync();

}