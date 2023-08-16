using FluentValidation;

namespace AnSumeri.API.Application.Categories.Commands.Delete;

using Models.Mediatr;

public class DeleteCategoryValidator : CategoryBaseRequestValidator<DeleteCategory, ResultDto<DeleteCategoryResult>>
{
    public DeleteCategoryValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Category doesn't exist");
    }
}
