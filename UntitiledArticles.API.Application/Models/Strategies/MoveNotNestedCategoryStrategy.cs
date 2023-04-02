﻿using MediatR;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Models.Strategies;

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
        GetCategoryResponse categoryToMoveResponse = await _mediator.Send(new GetCategory(id));
        ValidateGetCategoryResponses(categoryToMoveResponse);
        
        await _categoryRepository.UpdateAsync(new Category()
        {
            Id = categoryToMoveResponse.Result.Id,
            Name = categoryToMoveResponse.Result.Name,
            ParentId = moveToCategoryId
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