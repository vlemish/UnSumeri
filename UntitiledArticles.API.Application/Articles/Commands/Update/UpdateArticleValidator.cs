namespace UntitiledArticles.API.Application.Articles.Commands.Update;

using FluentValidation;

public class UpdateArticleValidator : AbstractValidator<UpdateArticle>
{
    public UpdateArticleValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive integer greater than 0!");
        RuleFor(p => p.Content).NotEmpty()
            .WithMessage("Content can't be null or empty!");
        RuleFor(p => p.Title).NotEmpty()
            .WithMessage("Title can't be null or empty!");
    }
}
