using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.Move.Statuses;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.OperationStatuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Move
{
    public class MoveCategoryHandler : IRequestHandler<MoveCategory, MoveCategoryResponse>
    {
        private readonly ILogger<MoveCategoryHandler> _logger;
        private ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;

        public MoveCategoryHandler(ILogger<MoveCategoryHandler> logger, ICategoryRepository categoryRepository, IMediator mediator)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _mediator = mediator;
        }

        public async Task<MoveCategoryResponse> Handle(MoveCategory request, CancellationToken cancellationToken)
        {
            GetCategoryResponse response = await _mediator.Send(new GetCategory(request.Id));
            if (response.Status.Status != OperationStatuses.OperationStatusValue.OK)
            {
                ReportNotFound(response.Status);
            }

            await _categoryRepository.UpdateAsync(CreateCategoryForUpdate(request, response.Result));
            return ReportSuccess(request);
        }

        private MoveCategoryResponse ReportNotFound(IOperationStatus status)
        {
            _logger.LogDebug($"Failed to move category: {status.Message}");
            return new(status);
        }

        private MoveCategoryResponse ReportSuccess(MoveCategory request)
        {
            _logger.LogDebug($"{nameof(MoveCategory)} where Id = {request.Id} and Parent Id = {request.ParentId} was successfully handled");
            return new(new MoveCategorySuccess(request.Id, request.ParentId));
        }

        private Category CreateCategoryForUpdate(MoveCategory request, GetCategoryResult getCategoryResult) =>
            new()
            {
                Id = request.Id,
                Name = getCategoryResult.Name,
                ParentId = request.ParentId
            };
    }
}
