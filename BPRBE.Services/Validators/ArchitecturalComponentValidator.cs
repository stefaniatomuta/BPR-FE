using BPRBE.Services.Models;
using FluentValidation;

namespace BPRBE.Services.Validators;

public class ArchitecturalComponentValidator : AbstractValidator<ArchitecturalComponent>
{
    public ArchitecturalComponentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Component name is required");
    }
}