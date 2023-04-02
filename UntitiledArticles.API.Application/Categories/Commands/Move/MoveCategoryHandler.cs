using MediatR;
using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;
using UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;
using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Move
{
    public class MoveCategoryHandler : IRequestHandler<MoveCategory, MoveCategoryResponse>
    {
        private readonly ILogger<MoveCategoryHandler> _logger;
        private readonly IMediator _mediator;

        public MoveCategoryHandler(ILogger<MoveCategoryHandler> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<MoveCategoryResponse> Handle(MoveCategory request, CancellationToken cancellationToken)
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
            MoveAsRootResponse response =
                await _mediator.Send(new MoveAsRoot.MoveAsRoot(request.Id), cancellationToken);
            return response.Status;
        }

        private async Task<IOperationStatus> PerformMoveAsSubCategoryCommand(MoveCategory request,
            CancellationToken cancellationToken)
        {
            MoveAsSubCategoryResponse response =
                await _mediator.Send(new MoveAsSubCategory.MoveAsSubCategory(request.Id, request.MoveToId.Value),
                    cancellationToken);
            return response.Status;
        }
    }
}