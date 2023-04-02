using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;

public class MoveAsSubCategoryValidator : AbstractValidator<MoveAsSubCategory>
{
    public MoveAsSubCategoryValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("Id should be greater than zero!");
        RuleFor(p => p.MoveToId).GreaterThan(0).WithMessage("Parent Id should be greater than zero!");
    }
}