using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;

public class MoveAsRootValidator : AbstractValidator<MoveAsRoot>
{
    public MoveAsRootValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0)
            .WithMessage("Id should be positive number greater than zero!");
    }
}