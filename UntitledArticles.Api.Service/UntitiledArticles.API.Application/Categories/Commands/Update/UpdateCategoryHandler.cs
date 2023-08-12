using System.Linq.Expressions;

using MediatR;

using UntitiledArticles.API.Application.Categories.Commands.Update.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.FindMany;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitiledArticles.API.Application.OperationStatuses.Shared.Categories;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Update;

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
        ResultDto<FindManyByFilterResult> filterResult =
            await _mediator.Send(new FindManyByFilter(CreateFilterExpression(request)), cancellationToken);
        var validationResult = ValidateCategory(request, filterResult);
        if (!validationResult.isValid)
        {
            return new(validationResult.operationStatus);
        }

        Category category = new()
        {
            Id = request.Id,
            UserId = request.UserId,
            Name = request.Name,
        };

        await _categoryRepository.UpdateAsync(category);
        return ReportSuccess(request);
    }

    private (bool isValid, IOperationStatus operationStatus) ValidateCategory(UpdateCategory request,
        ResultDto<FindManyByFilterResult> resultDto)
    {
        if (resultDto.OperationStatus.Status == OperationStatusValue.NoContent)
        {
            return new(false, resultDto.OperationStatus);
        }

        bool isNameSame = resultDto.Payload.Categories.Any(c => c.Name.Equals(request.Name) && c.Id == request.Id);
        if (isNameSame)
        {
            return new(false, new CategoryNotModified(request.Id, request.UserId, request.Name));
        }
        bool isNameReserved = resultDto.Payload.Categories.Any(c => c.Name.Equals(request.Name));
        if (isNameReserved)
        {
            return new(false, new DuplicateCategory(request.Name));
        }

        return new(true, resultDto.OperationStatus);
    }

    private Expression<Func<Category, bool>> CreateFilterExpression(UpdateCategory request) =>
        (c => c.UserId == request.UserId && c.Id == request.Id || c.Name == request.Name);

    private ResultDto ReportSuccess(UpdateCategory request) =>
        new(new UpdateCategorySuccess(request.Id));
}
