using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Models.Strategies;

public interface ICategoryMoveStrategy
{
    Task Move(int id, int? moveToCategoryId);
}