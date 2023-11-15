using BPR.Mediator.Models;
using BPR.Persistence.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace BPR.Mediator.Validators;

public class ValidatorService : IValidatorService
{
    private readonly IValidator<ArchitecturalModel> _architecturalModelValidator;
    private readonly IValidator<Rule> _ruleValidator;
    private readonly IValidator<ResultModel> _resultValidator;

    public ValidatorService(IValidator<ArchitecturalModel> architecturalModelValidator, IValidator<Rule> ruleValidator, IValidator<ResultModel> resultValidator)
    {
        _architecturalModelValidator = architecturalModelValidator;
        _ruleValidator = ruleValidator;
        _resultValidator = resultValidator;
    }

    public async Task<Result> ValidateArchitecturalModelAsync(ArchitecturalModel model)
    {
        var result = await _architecturalModelValidator.ValidateAsync(model);
        if (result.IsValid) return new Result(true);
        var errorMessages = result.Errors.Select(x=> x.ErrorMessage).ToList();
        return new Result(false, errorMessages);
    }
    public async Task<Result> ValidateRuleAsync(Rule rule)
    {
        var result = await _ruleValidator.ValidateAsync(rule);
        return HandleValidatorResults(result);
    }

    public async Task<Result> ValidateResultAsync(ResultModel resultModel)
    {
        var result = await _resultValidator.ValidateAsync(resultModel);
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
