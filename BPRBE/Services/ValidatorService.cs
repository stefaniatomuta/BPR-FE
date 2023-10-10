using BPR.Models.Persistence;
using BPRBE.Models.Persistence;
using FluentValidation;
using FluentValidation.Results;

namespace BPRBE.Validators;

public class ValidatorService : IValidatorService
{
    private readonly IValidator<ArchitecturalModel> _architecturalModelValidator;
    private readonly IValidator<Rule> _ruleValidator;

    public ValidatorService(IValidator<ArchitecturalModel> architecturalModelValidator, IValidator<Rule> ruleValidator)
    {
        _architecturalModelValidator = architecturalModelValidator;
        _ruleValidator = ruleValidator;
    }

    public async Task<Result> ValidateArchitecturalModelAsync(ArchitecturalModel model)
    {
        var result = await _architecturalModelValidator.ValidateAsync(model);
        return HandleValidatorResults(result);
    }

    public async Task<Result> ValidateRuleAsync(Rule rule)
    {
        var result = await _ruleValidator.ValidateAsync(rule);
        return HandleValidatorResults(result);
    }

    private Result HandleValidatorResults(ValidationResult? result)
    {
        if (result != null)
        {
            if (result.IsValid) return new Result(true);
            var errorMessages = result.Errors.Select(x=> x.ErrorMessage).ToList();
            return new Result(false, errorMessages);
        }
        return new Result(false, "Something went wrong");
    }
}
