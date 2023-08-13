using System.Diagnostics;
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
            CategoryBaseRequest<ResultDto> moveRequest = GetMoveOperation(request);
            ResultDto result = await _mediator.Send(moveRequest, cancellationToken);
            return new(result?.OperationStatus);
        }

        private CategoryBaseRequest<ResultDto> GetMoveOperation(MoveCategory request) =>
            request.MoveToId.HasValue
                ? new MoveAsSubCategory.MoveAsSubCategory(request.Id, request.UserId, request.MoveToId.Value)
                : new MoveAsRoot.MoveAsRoot(request.Id, request.UserId);
    }
}
