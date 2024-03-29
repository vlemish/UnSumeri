﻿using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Application.OperationStatuses;
using MediatR;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Models.Strategies;

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

    public async Task Move(int id, string userId, int? moveToCategoryId)
    {
        ResultDto<GetCategoryByIdResult>  categoryToMoveResponse = await _mediator.Send(new GetCategoryById(id, userId));
        ValidateGetCategoryResponses(categoryToMoveResponse);

        await _categoryRepository.UpdateAsync(new Category()
        {
            Id = categoryToMoveResponse.Payload.Id,
            UserId = userId,
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
