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
            GetCategoryResponse categoryResponse = await _mediator.Send(new GetCategory(request.Id));
            if (categoryResponse.Status.Status != OperationStatuses.OperationStatusValue.OK)
            {
                ReportNotFound(categoryResponse.Status);
            }
            bool isParentCategoryValid = await ValidateParentCategoryAsync(request);
            if (!isParentCategoryValid)
            {
                return ReportParentCategoryNotExist(request);
            }

            await _categoryRepository.UpdateAsync(CreateCategoryForUpdate(request, categoryResponse.Result));
            return ReportSuccess(request);
        }

        private async Task<bool> ValidateParentCategoryAsync(MoveCategory request)
        {
            if (!request.ParentId.HasValue)
            {
                return true;
            }

            var parentCategoryResponse = await _mediator.Send(new GetCategory(request.ParentId.Value));
            return parentCategoryResponse.Status.Status == OperationStatusValue.OK;
        }

        private MoveCategoryResponse ReportParentCategoryNotExist(MoveCategory request)
        {
            _logger.LogDebug($"Failed to move category where Id = {request.Id} and Parent Id = {request.ParentId}: Parent Category doesn't exist!");
            return new(new MoveCategoryParentCategoryNotExist(request.Id, request.ParentId));
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
