using FluentValidation;

namespace AnSumeri.API.Application.Articles.Queries.FindArticlesByPattern;

public class FindArticlesByPatternValidator : AbstractValidator<FindArticlesByPattern>
{
    public FindArticlesByPatternValidator()
    {
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("User Id should be present!");
        RuleFor(p => p.SearchPattern)
            .Must(sp => String.IsNullOrEmpty(sp) && sp.Length >= 3)
            .WithMessage("Search pattern should be non empty string with length greater than 3");
    }
}
