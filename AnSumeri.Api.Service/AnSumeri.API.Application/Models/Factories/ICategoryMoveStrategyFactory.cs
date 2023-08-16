using AnSumeri.API.Application.Models.Strategies;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Models.Factories;

public interface ICategoryMoveStrategyFactory
{
    ICategoryMoveStrategy CreateCategoryMoveStrategy(int categoryToMoveId, int? destinationParentId);
}
