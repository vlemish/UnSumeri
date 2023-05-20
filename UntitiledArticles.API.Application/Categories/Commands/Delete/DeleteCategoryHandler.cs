using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.Delete.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.GetById;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete;

using Models.Mediatr;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategory, ResultDto>
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

    public async Task<ResultDto> Handle(DeleteCategory request, CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> response = await _mediator.Send(new GetCategoryById(request.Id), cancellationToken);
        Category category = _mapper.Map<Category>(response.Payload);
        if (category is null)
        {
            return ReportCategoryNotFound(request);
        }

        await _repository.DeleteAsync(category);

        return ReportSuccess(request);
    }

    private ResultDto ReportCategoryNotFound(DeleteCategory request)
    {
        DeleteCategoryNotFound operationStatus = new(request.Id);
        return new(operationStatus);
    }

    private ResultDto ReportSuccess(DeleteCategory request)
    {
        DeleteCategorySuccess operationStatus = new(request.Id);
        return new(operationStatus);
    }

    private Category CreateCategory(GetCategoryByIdResult categoryResult) =>
        new()
        {
            Id = categoryResult.Id,
            Name = categoryResult.Name,
            ParentId = categoryResult.ParentId,
        };
}
