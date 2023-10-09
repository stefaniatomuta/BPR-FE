using BPRBE.Models.Persistence;
using FluentValidation;

namespace BPRBE.Validators;

public class RuleValidator : AbstractValidator<Rule>
{
    public RuleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Rule must contain a name");
    }
}