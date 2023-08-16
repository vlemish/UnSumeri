using AnSumeri.API.Application.Models.Mediatr;
using FluentValidation;

namespace AnSumeri.API.Application.Categories.Commands.Update;

public class UpdateCategoryValidator : CategoryBaseRequestValidator<UpdateCategory, ResultDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Category Name can't null or empty");
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Category Id should be a valid positive integer");
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("Category Name can't null or empty");
    }
}
