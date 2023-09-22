using BPRBE.Models.Persistence;
using FluentValidation;

namespace BPRBE.Validators;

public class ValidatorService : IValidatorService
{
    private readonly IValidator<ArchitecturalModel> _validator;


    public ValidatorService(IValidator<ArchitecturalModel> validator)
    {
        _validator = validator;
    }

    public async Task<Result> ValidateArchitecturalModelAsync(ArchitecturalModel model)
    {
        var result = await _validator.ValidateAsync(model);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(x=> x.ErrorMessage).ToList();
            return new Result(false, errorMessages);
        }
        var duplicates = model.Components.GroupBy(x => x.Name).Any(n => n.Count() > 1);
        return duplicates ? new Result(false, "Duplicate names of components are not allowed") : new Result(true);
    }
}