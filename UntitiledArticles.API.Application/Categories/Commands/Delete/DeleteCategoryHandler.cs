using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.Delete.Statuses;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.OperationStatuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategory, DeleteCategoryResponse>
{
    private readonly ILogger<DeleteCategoryHandler> _logger;
    private readonly ICategoryRepository _repository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DeleteCategoryHandler(ILogger<DeleteCategoryHandler> logger, ICategoryRepository repository, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<DeleteCategoryResponse> Handle(DeleteCategory request, CancellationToken cancellationToken)
    {
        GetCategoryResponse response = await _mediator.Send(new GetCategory(request.Id), cancellationToken);
        Category category = _mapper.Map<Category>(response.Result);
        if (category is null)
        {
            return ReportCategoryNotFound(request);
        }
        //if (getCategoryResponse.Status.Status != OperationStatusValue.OK)
        //{
        //    return ReportCategoryNotFound(request);
        //}

        await _repository.DeleteAsync(category);

        return ReportSuccess(request);
    }

    private DeleteCategoryResponse ReportCategoryNotFound(DeleteCategory request)
    {
        DeleteCategoryNotFound operationStatus = new(request.Id);
        _logger.LogError(operationStatus.Message);
        return new(operationStatus);
    }

    private DeleteCategoryResponse ReportSuccess(DeleteCategory request)
    {
        DeleteCategorySuccess operationStatus = new(request.Id);
        _logger.LogInformation(operationStatus.Message);
        return new(operationStatus);
    }

    private Category CreateCategory(GetCategoryResult categoryResult) =>
        new()
        {
            Id = categoryResult.Id,
            Name = categoryResult.Name,
            ParentId = categoryResult.ParentId,
        };
}