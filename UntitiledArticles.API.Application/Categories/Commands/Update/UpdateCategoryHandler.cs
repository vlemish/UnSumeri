using MediatR;

using UntitiledArticles.API.Application.Categories.Commands.Update.Statuses;

using UntitiledArticles.API.Application.Categories.Queries.GetById;

using UntitledArticles.API.Domain.Contracts;

using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Update;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategory, UpdateCategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository, IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<UpdateCategoryResponse> Handle(UpdateCategory request, CancellationToken cancellationToken)
    {
        GetCategoryByIdResponse getCategoryByResponse = await _mediator.Send(new GetCategoryById(request.Id), cancellationToken);
        if (getCategoryByResponse.Status.Status != OperationStatuses.OperationStatusValue.OK)
        {
            return ReportNotFound(request);
        }

        Category category = new()
        {
            Id = getCategoryByResponse.Result.Id,
            ParentId = getCategoryByResponse.Result.ParentId,
            Name = request.Name,
        };

        await _categoryRepository.UpdateAsync(category);
        return ReportSuccess(request);
    }

    private UpdateCategoryResponse ReportNotFound(UpdateCategory request) =>
        new(new UpdateCategoryNotFound(request.Id));

    private UpdateCategoryResponse ReportSuccess(UpdateCategory request) =>
        new(new UpdateCategorySuccess(request.Id));
}