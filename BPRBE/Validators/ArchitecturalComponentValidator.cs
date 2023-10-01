using BPR.Models.Persistence;
using BPRBE.Models.Persistence;
using FluentValidation;

namespace BPRBE.Validators;

public class ArchitecturalComponentValidator : AbstractValidator<MongoArchitecturalComponent>
{
    public ArchitecturalComponentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Component name is required");
    }
}