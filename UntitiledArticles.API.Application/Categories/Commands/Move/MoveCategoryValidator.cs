using FluentValidation;

namespace UntitiledArticles.API.Application.Categories.Commands.Move
{
    public class MoveCategoryValidator : AbstractValidator<MoveCategory>
    {
        public MoveCategoryValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("Id should be greater than zero!");
            RuleFor(p => p.ParentId).Must(p => !p.HasValue || p.Value > 0).WithMessage("Parent Id should be either null or greater than zero!");
        }
    }
}
