using BPR.Persistence.Utils;
using BPRBE.Services.Models;

namespace BPRBE.Services.Services;

public interface IRuleService
{
    public Task<Result> AddRuleAsync(Rule rule);
    public Task<IList<Rule>> GetRulesAsync();
}