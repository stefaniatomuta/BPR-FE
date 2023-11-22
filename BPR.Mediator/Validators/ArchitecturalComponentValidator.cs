using BPR.Model.Architectures;
using FluentValidation;

namespace BPR.Mediator.Validators;

public class ArchitecturalComponentValidator : AbstractValidator<ArchitecturalComponent>
{
    public ArchitecturalComponentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Component name is required");
    }
}