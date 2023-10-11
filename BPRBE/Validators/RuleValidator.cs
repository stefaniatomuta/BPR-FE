using BPRBE.Models;
using FluentValidation;

namespace BPRBE.Validators;

public class RuleValidator : AbstractValidator<Rule>
{
    public RuleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("RuleCollection must contain a name");
    }
}