using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;

using Models.Mediatr;

public class MoveAsRootValidator : CategoryBaseRequestValidator<MoveAsRoot, ResultDto>
{
    public MoveAsRootValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0)
            .WithMessage("Id should be positive number greater than zero!");
    }
}
