using BPRBE.Models.Persistence;
using FluentValidation;

namespace BPRBE.Validators;

public class ArchitecturalModelValidator : AbstractValidator<ArchitecturalModel>
{
    public ArchitecturalModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Model name is required");
        RuleFor(x => x.Components).NotEmpty();
        RuleForEach(x => x.Components).NotEmpty().WithMessage("Model cannot have no components");
        RuleForEach(x => x.Components).SetValidator(new ArchitecturalComponentValidator());
    }
}