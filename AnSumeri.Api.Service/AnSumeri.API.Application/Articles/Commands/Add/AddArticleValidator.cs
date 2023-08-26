using FluentValidation;

namespace AnSumeri.API.Application.Articles.Commands.Add;

public class AddArticleValidator : AbstractValidator<AddArticle>
{
    public AddArticleValidator()
    {
        RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category Id should be greater than 0");
        RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("Title can't be empty or null");
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("User Id should be present");
    }
}
