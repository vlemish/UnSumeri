using AutoMapper;
using FluentValidation.Results;
using MediatR;

using UntitiledArticles.API.Application.Categories.Commands.Delete.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitiledArticles.API.Application.OperationStatuses.Shared.Categories;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategory, ResultDto<DeleteCategoryResult>>
{
    private readonly ICategoryRepository _repository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DeleteCategoryHandler(ICategoryRepository repository, IMediator mediator, IMapper mapper)
    {
        _repository = repository;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ResultDto<DeleteCategoryResult>> Handle(DeleteCategory request,
        CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> response =
            await _mediator.Send(new GetCategoryById(request.Id, request.UserId), cancellationToken);
        if (response.OperationStatus.Status != OperationStatusValue.OK)
        {
            return ReportCategoryNotFound(request);
        }

        Category category = _mapper.Map<Category>(response.Payload);
        await _repository.DeleteAsync(category);
        return ReportSuccess(request);
    }

    // private (bool isValid, IOperationStatus operationStatus) ValidateCategory(GetCategoryByIdResult result)
    // {
    //
    // }

    private ResultDto<DeleteCategoryResult> ReportCategoryNotFound(DeleteCategory request)
    {
        CategoryNotFound operationStatus = new(request.Id);
        return new(operationStatus, null);
    }

    private ResultDto<DeleteCategoryResult> ReportSuccess(DeleteCategory request)
    {
        DeleteCategorySuccess operationStatus = new(request.Id);
        return new(operationStatus, new(request.Id));
    }
}
