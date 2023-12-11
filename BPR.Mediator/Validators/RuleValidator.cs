using BPR.Model.Rules;
using FluentValidation;

namespace BPR.Mediator.Validators;

public class RuleValidator : AbstractValidator<Rule>
{
    public RuleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("RuleCollection must contain a name");
    }
}