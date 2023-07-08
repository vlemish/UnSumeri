using UntitiledArticles.API.Application.Models.Strategies;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Models.Factories;

public interface ICategoryMoveStrategyFactory
{
    ICategoryMoveStrategy CreateCategoryMoveStrategy(Category categoryToMove, int? moveToId);
}