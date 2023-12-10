using BPR.Model.Architectures;
using FluentValidation;

namespace BPR.Mediator.Validators;

public class ArchitectureComponentValidator : AbstractValidator<ArchitectureComponent>
{
    public ArchitectureComponentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Component name is required");
    }
}