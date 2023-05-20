using MediatR;
using Microsoft.Extensions.Logging;
using UntitiledArticles.API.Application.Categories.Commands.Move.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;

using Models.Mediatr;

public class MoveAsRootHandler : IRequestHandler<MoveAsRoot, ResultDto>
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

    public async Task<ResultDto> Handle(MoveAsRoot request, CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> categoryResponse =
            await _mediator.Send(new GetCategoryById(request.Id), cancellationToken);
        if (categoryResponse.OperationStatus.Status != OperationStatuses.OperationStatusValue.OK)
        {
            return ReportNotFound(request, categoryResponse.OperationStatus);
        }

        ICategoryMoveStrategy categoryMoveStrategy = _categoryMoveStrategyFactory.CreateCategoryMoveStrategy(
            new Category() { Id = request.Id, Name = categoryResponse.Payload.Name, ParentId = null, }, moveToId: null);
        await categoryMoveStrategy.Move(request.Id, null);
        return ReportSuccess(request);
    }

    private ResultDto ReportSuccess(MoveAsRoot request) =>
        new(new MoveCategorySuccess(request.Id, null));

    private ResultDto ReportNotFound(MoveAsRoot request, IOperationStatus status) =>
        new(status);
}
