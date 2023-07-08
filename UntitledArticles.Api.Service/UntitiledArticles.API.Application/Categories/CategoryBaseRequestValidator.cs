namespace UntitiledArticles.API.Application.Categories;

using FluentValidation;

public abstract class CategoryBaseRequestValidator<TRequest,TResponse> : AbstractValidator<TRequest> where
    TResponse : class
    where TRequest : CategoryBaseRequest<TResponse>
{
    public CategoryBaseRequestValidator()
    {
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("UserId can't be null or empty!");
    }
}
