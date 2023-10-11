using BPR.Persistence.Utils;
using BPRBE.Models;

namespace BPRBE.Validators;

public interface IValidatorService
{
    public Task<Result> ValidateArchitecturalModelAsync(ArchitecturalModel model);
    public Task<Result> ValidateRuleAsync(Rule rule);
}