using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete;

public class DeleteCategoryValidator : AbstractValidator<DeleteCategory>
{
    public DeleteCategoryValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Category doesn't exist");
    }
}