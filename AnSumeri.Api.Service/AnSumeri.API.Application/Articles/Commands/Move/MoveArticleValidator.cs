using FluentValidation;

namespace AnSumeri.API.Application.Articles.Commands.Move;

public class MoveArticleValidator : AbstractValidator<MoveArticle>
{
    public MoveArticleValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0!");
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("User id should be present!");
        RuleFor(p => p.CategoryToMoveId)
            .GreaterThan(0)
            .WithMessage("Category to move must be greater than 0!");
    }
}
