using MediatR;

using UntitiledArticles.API.Application.Models.Strategies;

using UntitledArticles.API.Domain.Contracts;

using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Models.Factories;

public class CategoryMoveStrategyFactory : ICategoryMoveStrategyFactory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public CategoryMoveStrategyFactory(ICategoryRepository categoryRepository, IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public ICategoryMoveStrategy CreateCategoryMoveStrategy(Category categoryToMove, int? moveToParentId)
    {
        bool isPreviousParentMovedToSubcategory = categoryToMove.ParentId == moveToParentId;
        if (isPreviousParentMovedToSubcategory)
        {
            return new MoveNestedCategoryStrategy(_categoryRepository, _mediator);
        }

        return new MoveNotNestedCategoryStrategy(_categoryRepository, _mediator);
    }
}