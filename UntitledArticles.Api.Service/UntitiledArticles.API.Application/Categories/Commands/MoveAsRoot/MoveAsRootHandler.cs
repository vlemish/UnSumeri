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
    private readonly ICategoryMoveStrategyFactory _categoryMoveStrategyFactory;
    private readonly IMediator _mediator;

    public MoveAsRootHandler(ICategoryMoveStrategyFactory categoryMoveStrategyFactory,
        IMediator mediator)
    {
        _categoryMoveStrategyFactory = categoryMoveStrategyFactory;
        _mediator = mediator;
    }

    public async Task<ResultDto> Handle(MoveAsRoot request, CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> categoryResponse =
            await _mediator.Send(new GetCategoryById(request.Id, request.UserId), cancellationToken);
        if (categoryResponse.OperationStatus.Status != OperationStatuses.OperationStatusValue.OK)
        {
            return ReportNotFound(request, categoryResponse.OperationStatus);
        }

        if (!categoryResponse.Payload.ParentId.HasValue)
        {
            return ReportNotModified(request);
        }

        ICategoryMoveStrategy categoryMoveStrategy =
            _categoryMoveStrategyFactory.CreateCategoryMoveStrategy(categoryResponse.Payload.Id, null);
        await categoryMoveStrategy.Move(request.Id, request.UserId, null);
        return ReportSuccess(request);
    }

    private ResultDto ReportSuccess(MoveAsRoot request) =>
        new(new MoveCategorySuccess(request.Id, null));

    private ResultDto ReportNotModified(MoveAsRoot request) =>
        new(new MoveCategoryNotModified(request.Id, null));

    private ResultDto ReportNotFound(MoveAsRoot request, IOperationStatus status) =>
        new(status);
}
