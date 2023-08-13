using MediatR;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Models.Strategies;

using Mediatr;

public class MoveNestedCategoryStrategy : ICategoryMoveStrategy
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public MoveNestedCategoryStrategy(ICategoryRepository categoryRepository, IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task Move(int id, string userId, int? moveToCategoryId)
    {
        if (!moveToCategoryId.HasValue)
        {
            throw new ArgumentNullException($"{nameof(moveToCategoryId)} was null!");
        }

        ResultDto<GetCategoryByIdResult> parentCategoryResponse =
            await _mediator.Send(new GetCategoryById(moveToCategoryId.Value, userId));
        ResultDto<GetCategoryByIdResult> categoryToMoveResponse = await _mediator.Send(new GetCategoryById(id, userId));

        ValidateGetCategoryResponses(parentCategoryResponse, categoryToMoveResponse);

        // update new parent not to relate to category to move
        await _categoryRepository.UpdateAsync(new Category()
        {
            Id = parentCategoryResponse.Payload.Id,
            UserId = userId,
            Name = parentCategoryResponse.Payload.Name,
            ParentId = categoryToMoveResponse.Payload.ParentId
        });
        await _categoryRepository.UpdateAsync(new Category()
        {
            Id = categoryToMoveResponse.Payload.Id,
            UserId = userId,
            Name = categoryToMoveResponse.Payload.Name,
            ParentId = parentCategoryResponse.Payload.Id
        });
    }

    private void ValidateGetCategoryResponses(params ResultDto<GetCategoryByIdResult>[] responses) =>
        responses
            .Where(response => !IsGetCategoryResponseValid(response))
            .ToList()
            .ForEach(r => throw new ArgumentOutOfRangeException(r.OperationStatus.Message));

    private bool IsGetCategoryResponseValid(ResultDto<GetCategoryByIdResult> response) =>
        response?.OperationStatus?.Status == OperationStatusValue.OK
        && response.Payload is not null;
}
