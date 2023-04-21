using MediatR;
using Microsoft.Extensions.Logging;
using UntitiledArticles.API.Application.Categories.Commands.Move.Statuses;
using UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;

public class MoveAsSubCategoryHandler : IRequestHandler<MoveAsSubCategory, MoveAsSubCategoryResponse>
{
    private readonly ILogger<MoveAsSubCategoryHandler> _logger;
    private readonly ICategoryMoveStrategyFactory _categoryMoveStrategyFactory;
    private readonly IMediator _mediator;

    public MoveAsSubCategoryHandler(ILogger<MoveAsSubCategoryHandler> logger,
        ICategoryMoveStrategyFactory categoryMoveStrategyFactory,
        IMediator mediator)
    {
        _logger = logger;
        _categoryMoveStrategyFactory = categoryMoveStrategyFactory;
        _mediator = mediator;
    }

    public async Task<MoveAsSubCategoryResponse> Handle(MoveAsSubCategory request, CancellationToken cancellationToken)
    {
        GetCategoryByIdResponse categoryResponse = await _mediator.Send(new GetCategoryById(request.Id), cancellationToken);
        if (categoryResponse.Status.Status != OperationStatusValue.OK)
        {
            return ReportNotFound(categoryResponse.Status);
        }

        GetCategoryByIdResponse categoryToMoveResponse =
            await _mediator.Send(new GetCategoryById(request.MoveToId), cancellationToken);
        if (categoryToMoveResponse.Status.Status != OperationStatusValue.OK)
        {
            return ReportParentCategoryNotExist(request);
        }
        
        ICategoryMoveStrategy categoryMoveStrategy = _categoryMoveStrategyFactory.CreateCategoryMoveStrategy(new Category()
        {
            Id = request.Id,
            Name = categoryResponse.Result.Name,
            ParentId = request.MoveToId
        }, request.Id);
        await categoryMoveStrategy.Move(request.Id, request.MoveToId);
        return ReportSuccess(request);
    }
    
    private MoveAsSubCategoryResponse ReportParentCategoryNotExist(MoveAsSubCategory request)
    {
        _logger.LogDebug(
            $"Failed to move category where Id = {request.Id} and Parent Id = {request}: Parent Category doesn't exist!");
        return new(new MoveCategoryParentCategoryNotExist(request.Id, request.MoveToId));
    }

    private MoveAsSubCategoryResponse ReportNotFound(IOperationStatus status)
    {
        _logger.LogDebug($"Failed to move category: {status.Message}");
        return new(status);
    }
    
    private MoveAsSubCategoryResponse ReportSuccess(MoveAsSubCategory request)
    {
        _logger.LogDebug(
            $"{nameof(MoveAsSubCategory)} where Id = {request.Id} and Parent Id = {request.MoveToId} was successfully handled");
        return new(new MoveCategorySuccess(request.Id, request.MoveToId));
    }
}