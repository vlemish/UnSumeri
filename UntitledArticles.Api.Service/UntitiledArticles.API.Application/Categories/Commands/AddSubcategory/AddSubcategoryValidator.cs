using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    using Models.Mediatr;

    public class AddSubcategoryValidator : CategoryBaseRequestValidator<AddSubcategory, ResultDto<AddSubcategoryResult>>
    {
        public AddSubcategoryValidator()
        {
            RuleFor(p => p.Name).NotEmpty()
                .WithMessage("Name was null or empty!");
            RuleFor(p => p.ParentId).GreaterThan(0).WithMessage("Id can't be negative number or zero!");
        }
    }
}
