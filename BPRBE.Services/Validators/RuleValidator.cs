using BPRBE.Services.Models;
using FluentValidation;

namespace BPRBE.Services.Validators;

public class RuleValidator : AbstractValidator<Rule>
{
    public RuleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("RuleCollection must contain a name");
    }
}