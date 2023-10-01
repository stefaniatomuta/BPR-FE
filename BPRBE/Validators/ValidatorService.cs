using BPR.Models.Persistence;
using BPRBE.Models.Persistence;
using FluentValidation;

namespace BPRBE.Validators;

public class ValidatorService : IValidatorService
{
    private readonly IValidator<MongoArchitecturalModel> _validator;
    
    public ValidatorService(IValidator<MongoArchitecturalModel> validator)
    {
        _validator = validator;
    }

    public async Task<Result> ValidateArchitecturalModelAsync(MongoArchitecturalModel model)
    {
        var result = await _validator.ValidateAsync(model);
        if (result.IsValid) return new Result(true);
        var errorMessages = result.Errors.Select(x=> x.ErrorMessage).ToList();
        return new Result(false, errorMessages);
    }
}