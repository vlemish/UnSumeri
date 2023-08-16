using FluentValidation;

namespace AnSumeri.API.Application.Categories.Commands.Move
{
    using Models.Mediatr;

    public class MoveCategoryValidator : CategoryBaseRequestValidator<MoveCategory, ResultDto>
    {
        public MoveCategoryValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("Id should be greater than zero!");
            RuleFor(p => p.MoveToId).Must(p => !p.HasValue || p.Value > 0).WithMessage("MoveToId Id should be either null or greater than zero!");
        }
    }
}
