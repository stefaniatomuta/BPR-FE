using BPR.Mediator.Utils;
using BPR.Model.Architectures;

namespace BPR.Mediator.Validators;

public interface IValidatorService
{
    public Task<Result> ValidateArchitecturalModelAsync(ArchitecturalModel model);
    public Task<Result> ValidateRuleAsync(Rule rule);
}