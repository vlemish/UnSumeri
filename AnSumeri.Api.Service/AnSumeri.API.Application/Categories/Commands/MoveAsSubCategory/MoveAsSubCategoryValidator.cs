using FluentValidation;

namespace AnSumeri.API.Application.Categories.Commands.MoveAsSubCategory;

using Models.Mediatr;

public class MoveAsSubCategoryValidator : CategoryBaseRequestValidator<MoveAsSubCategory, ResultDto>
{
    public MoveAsSubCategoryValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("Id should be greater than zero!");
        RuleFor(p => p.MoveToId).GreaterThan(0).WithMessage("Parent Id should be greater than zero!");
    }
}
