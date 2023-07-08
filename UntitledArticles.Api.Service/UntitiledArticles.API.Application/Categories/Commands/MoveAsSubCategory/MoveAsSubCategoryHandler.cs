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

using Models.Mediatr;

public class MoveAsSubCategoryHandler : IRequestHandler<MoveAsSubCategory, ResultDto>
{
    private readonly ICategoryMoveStrategyFactory _categoryMoveStrategyFactory;
    private readonly IMediator _mediator;

    public MoveAsSubCategoryHandler(ICategoryMoveStrategyFactory categoryMoveStrategyFactory,
        IMediator mediator)
    {
        _categoryMoveStrategyFactory = categoryMoveStrategyFactory;
        _mediator = mediator;
    }

    public async Task<ResultDto> Handle(MoveAsSubCategory request, CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> categoryResponse =
            await _mediator.Send(new GetCategoryById(request.Id, request.UserId), cancellationToken);
        if (categoryResponse.OperationStatus.Status != OperationStatusValue.OK)
        {
            return ReportNotFound(categoryResponse.OperationStatus);
        }

        ResultDto<GetCategoryByIdResult> categoryToMoveResponse =
            await _mediator.Send(new GetCategoryById(request.MoveToId, request.UserId), cancellationToken);
        if (categoryToMoveResponse.OperationStatus.Status != OperationStatusValue.OK)
        {
            return ReportParentCategoryNotExist(request);
        }

        ICategoryMoveStrategy categoryMoveStrategy = _categoryMoveStrategyFactory.CreateCategoryMoveStrategy(
            new Category() { Id = request.Id, Name = categoryResponse.Payload.Name, ParentId = request.MoveToId },
            request.Id);
        await categoryMoveStrategy.Move(request.Id, request.MoveToId);
        return ReportSuccess(request);
    }

    private ResultDto ReportParentCategoryNotExist(MoveAsSubCategory request) =>
        new(new MoveCategoryParentCategoryNotExist(request.Id, request.MoveToId));

    private ResultDto ReportNotFound(IOperationStatus status) =>
        new(status);

    private ResultDto ReportSuccess(MoveAsSubCategory request) =>
        new(new MoveCategorySuccess(request.Id, request.MoveToId));
}
