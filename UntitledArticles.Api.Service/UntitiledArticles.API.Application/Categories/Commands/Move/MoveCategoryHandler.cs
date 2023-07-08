using MediatR;
using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;
using UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;
using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Move
{
    using Models.Mediatr;

    public class MoveCategoryHandler : IRequestHandler<MoveCategory, ResultDto>
    {
        private readonly IMediator _mediator;

        public MoveCategoryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResultDto> Handle(MoveCategory request, CancellationToken cancellationToken)
        {
            if (request.MoveToId.HasValue)
            {
                var moveAsSubCategoryStatus = await PerformMoveAsSubCategoryCommand(request, cancellationToken);
                return new(moveAsSubCategoryStatus);
            }

            var moveAsRootStatus = await PerformMoveAsRootCommand(request, cancellationToken);
            return new(moveAsRootStatus);
        }

        private async Task<IOperationStatus> PerformMoveAsRootCommand(MoveCategory request,
            CancellationToken cancellationToken)
        {
            ResultDto response =
                await _mediator.Send(new MoveAsRoot.MoveAsRoot(request.Id, request.UserId), cancellationToken);
            return response.OperationStatus;
        }

        private async Task<IOperationStatus> PerformMoveAsSubCategoryCommand(MoveCategory request,
            CancellationToken cancellationToken)
        {
            ResultDto response =
                await _mediator.Send(new MoveAsSubCategory.MoveAsSubCategory(request.Id, request.UserId, request.MoveToId.Value),
                    cancellationToken);
            return response.OperationStatus;
        }
    }
}
