using BPR.Persistence.Utils;
using BPRBE.Services.Models;

namespace BPRBE.Services.Validators;

public interface IValidatorService
{
    public Task<Result> ValidateArchitecturalModelAsync(ArchitecturalModel model);
    public Task<Result> ValidateRuleAsync(Rule rule);
}