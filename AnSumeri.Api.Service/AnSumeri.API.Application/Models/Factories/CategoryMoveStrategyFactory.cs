using AnSumeri.API.Application.Models.Strategies;
using MediatR;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Models.Factories;

public class CategoryMoveStrategyFactory : ICategoryMoveStrategyFactory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public CategoryMoveStrategyFactory(ICategoryRepository categoryRepository, IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public ICategoryMoveStrategy CreateCategoryMoveStrategy(int categoryToMoveId, int? destinationParentId)
    {
        if (IsMoveToSubCategory(categoryToMoveId, destinationParentId))
        {
            return new MoveNestedCategoryStrategy(_categoryRepository, _mediator);
        }

        return new MoveNotNestedCategoryStrategy(_categoryRepository, _mediator);
    }

    private bool IsMoveToSubCategory(int categoryToMoveId, int? destinationParentId) =>
        categoryToMoveId == destinationParentId;
}
