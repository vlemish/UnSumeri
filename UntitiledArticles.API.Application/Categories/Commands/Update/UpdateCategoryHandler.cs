using MediatR;

using UntitiledArticles.API.Application.Categories.Commands.Update.Statuses;

using UntitiledArticles.API.Application.Categories.Queries.GetById;

using UntitledArticles.API.Domain.Contracts;

using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Update;

using Models.Mediatr;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategory, ResultDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository, IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<ResultDto> Handle(UpdateCategory request, CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> getCategoryByResponse = await _mediator.Send(new GetCategoryById(request.Id), cancellationToken);
        if (getCategoryByResponse.OperationStatus.Status != OperationStatuses.OperationStatusValue.OK)
        {
            return ReportNotFound(request);
        }

        Category category = new()
        {
            Id = getCategoryByResponse.Payload.Id,
            ParentId = getCategoryByResponse.Payload.ParentId,
            Name = request.Name,
        };

        await _categoryRepository.UpdateAsync(category);
        return ReportSuccess(request);
    }

    private ResultDto ReportNotFound(UpdateCategory request) =>
        new(new UpdateCategoryNotFound(request.Id));

    private ResultDto ReportSuccess(UpdateCategory request) =>
        new(new UpdateCategorySuccess(request.Id));
}
