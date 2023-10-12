using BPR.Mediator.Models;
using BPR.Persistence.Utils;

namespace BPR.Mediator.Validators;

public interface IValidatorService
{
    public Task<Result> ValidateArchitecturalModelAsync(ArchitecturalModel model);
    public Task<Result> ValidateRuleAsync(Rule rule);
}