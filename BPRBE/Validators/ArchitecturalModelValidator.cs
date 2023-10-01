using BPRBE.Models.Persistence;
using FluentValidation;

namespace BPRBE.Validators;

public class ArchitecturalModelValidator : AbstractValidator<MongoArchitecturalModel>
{
    public ArchitecturalModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Model name is required");
        RuleFor(x => x.Components).NotEmpty().WithMessage("Model cannot have no components");
        RuleForEach(x => x.Components).NotNull().NotEmpty().WithMessage("Components cannot be empty");
        RuleForEach(x => x.Components).SetValidator(new ArchitecturalComponentValidator());
        RuleFor(x => x.Components).Must(ContainDifferentComponentNames)
            .WithMessage("Duplicate names of components are not allowed");
    }

    private bool ContainDifferentComponentNames(IList<MongoArchitecturalComponent> components)
    {
        return !components.GroupBy(x => x.Name).Any(n => n.Count() > 1);
    }
}