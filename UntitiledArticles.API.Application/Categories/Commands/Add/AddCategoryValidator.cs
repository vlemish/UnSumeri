using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.Add
{
    public class AddCategoryValidator : AbstractValidator<AddCategory>
    {
        public AddCategoryValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Name can't be null or empty!");
        }
    }
}
