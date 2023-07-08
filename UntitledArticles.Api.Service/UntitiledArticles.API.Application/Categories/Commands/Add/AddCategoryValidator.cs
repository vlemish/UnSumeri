using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.Add
{
    using Models.Mediatr;

    public class AddCategoryValidator : CategoryBaseRequestValidator<AddCategory, ResultDto<AddCategoryResult>>
    {
        public AddCategoryValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Name can't be null or empty!");
        }
    }
}
