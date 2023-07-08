using MediatR;

using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.OperationStatuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Models.Strategies;

using Mediatr;

public class MoveNotNestedCategoryStrategy : ICategoryMoveStrategy
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public MoveNotNestedCategoryStrategy(ICategoryRepository categoryRepository, IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task Move(int id, int? moveToCategoryId)
    {
        ResultDto<GetCategoryByIdResult>  categoryToMoveResponse = await _mediator.Send(new GetCategoryById(id, null));
        ValidateGetCategoryResponses(categoryToMoveResponse);

        await _categoryRepository.UpdateAsync(new Category()
        {
            Id = categoryToMoveResponse.Payload.Id,
            Name = categoryToMoveResponse.Payload.Name,
            ParentId = moveToCategoryId
        });
    }

    private void ValidateGetCategoryResponses(params ResultDto<GetCategoryByIdResult> [] responses) =>
        responses
            .Where(response => !IsGetCategoryResponseValid(response))
            .ToList()
            .ForEach(r => throw new ArgumentOutOfRangeException(r.OperationStatus.Message));

    private bool IsGetCategoryResponseValid(ResultDto<GetCategoryByIdResult>  response) =>
        response?.OperationStatus?.Status == OperationStatusValue.OK
        && response.Payload is not null;
}
