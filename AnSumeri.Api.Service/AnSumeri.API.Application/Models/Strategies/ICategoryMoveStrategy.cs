using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Models.Strategies;

public interface ICategoryMoveStrategy
{
    Task Move(int id, string userId, int? moveToCategoryId);
}
