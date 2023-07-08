namespace UntitiledArticles.API.Application.Articles.Queries.GetOneById;

using FluentValidation;

public class GetOneArticleByIdValidator : AbstractValidator<GetOneArticleById>
{
    public GetOneArticleByIdValidator()
    {
        this.RuleFor(p => p.Id).GreaterThan(0)
            .WithMessage("Id has to be positive number greater than 0");
    }
}
