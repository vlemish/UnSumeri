using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.Move.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitiledArticles.API.Application.OperationStatuses;

using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;

public class MoveAsRootHandler : IRequestHandler<MoveAsRoot, MoveAsRootResponse>
{
    private readonly ILogger<MoveAsRootHandler> _logger;
    private readonly ICategoryMoveStrategyFactory _categoryMoveStrategyFactory;
    private readonly IMediator _mediator;
    
    public MoveAsRootHandler(ILogger<MoveAsRootHandler> logger,
        ICategoryMoveStrategyFactory categoryMoveStrategyFactory,
        IMediator mediator)
    {
        _logger = logger;
        _categoryMoveStrategyFactory = categoryMoveStrategyFactory;
        _mediator = mediator;
    }

    public async Task<MoveAsRootResponse> Handle(MoveAsRoot request, CancellationToken cancellationToken)
    {
        GetCategoryByIdResponse categoryResponse = await _mediator.Send(new GetCategoryById(request.Id), cancellationToken);
        if (categoryResponse.Status.Status != OperationStatuses.OperationStatusValue.OK)
        {
            return ReportNotFound(request, categoryResponse.Status);
        }
        
        ICategoryMoveStrategy categoryMoveStrategy = _categoryMoveStrategyFactory.CreateCategoryMoveStrategy(new Category()
        {
            Id = request.Id,
            Name = categoryResponse.Result.Name,
            ParentId = null,
        }, moveToId: null);
        await categoryMoveStrategy.Move(request.Id, null);
        return ReportSuccess(request);
    }

    private MoveAsRootResponse ReportSuccess(MoveAsRoot request)
    {
        _logger.LogDebug($"{nameof(MoveAsRoot)} command where Id = {request.Id} was successfully handled!");
        return new(new MoveCategorySuccess(request.Id, null));
    }
    
    private MoveAsRootResponse ReportNotFound(MoveAsRoot request, IOperationStatus status)
    {
        _logger.LogDebug($"Failed to move category where Id = {request.Id}: {status.Message}");
        return new(status);
    }
}