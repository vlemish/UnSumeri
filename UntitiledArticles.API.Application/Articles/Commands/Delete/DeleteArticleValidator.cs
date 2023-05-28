namespace UntitiledArticles.API.Application.Articles.Commands.Delete;

using FluentValidation;

public class DeleteArticleValidator : AbstractValidator<DeleteArticle>
{
    public DeleteArticleValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer greater than 0!");
    }
}
