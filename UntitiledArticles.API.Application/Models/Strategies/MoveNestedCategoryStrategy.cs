using MediatR;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Models.Strategies;

public class MoveNestedCategoryStrategy : ICategoryMoveStrategy
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public MoveNestedCategoryStrategy(ICategoryRepository categoryRepository, IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task Move(int id, int? moveToCategoryId)
    {
        if (!moveToCategoryId.HasValue)
        {
            throw new ArgumentNullException($"{nameof(moveToCategoryId)} was null!");
        }

        GetCategoryResponse parentCategoryResponse = await _mediator.Send(new GetCategory(moveToCategoryId.Value));
        GetCategoryResponse categoryToMoveResponse = await _mediator.Send(new GetCategory(id));

        ValidateGetCategoryResponses(parentCategoryResponse, categoryToMoveResponse);
        
        // update new parent not to relate to category to move
        await _categoryRepository.UpdateAsync(new Category()
        {
            Id = parentCategoryResponse.Result.Id,
            Name = parentCategoryResponse.Result.Name,
            ParentId = categoryToMoveResponse.Result.ParentId
        });
        await _categoryRepository.UpdateAsync(new Category()
        {
            Id = categoryToMoveResponse.Result.Id,
            Name = categoryToMoveResponse.Result.Name,
            ParentId = parentCategoryResponse.Result.Id
        });
    }

    private void ValidateGetCategoryResponses(params GetCategoryResponse[] responses) =>
        responses
            .Where(response => !IsGetCategoryResponseValid(response))
            .ToList()
            .ForEach(r => throw new ArgumentOutOfRangeException(r.Status.Message));

    private bool IsGetCategoryResponseValid(GetCategoryResponse response) =>
        response?.Status?.Status == OperationStatusValue.OK
        && response.Result is not null;
}