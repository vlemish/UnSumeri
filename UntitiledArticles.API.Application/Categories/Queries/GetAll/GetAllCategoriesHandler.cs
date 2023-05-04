using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Queries.GetAll.Statuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;

namespace UntitiledArticles.API.Application.Categories.Queries.GetAll;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories, GetAllCategoriesResponse>
{
    private readonly ILogger<GetAllCategoriesHandler> _logger;
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(ILogger<GetAllCategoriesHandler> logger, ICategoryRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetAllCategoriesResponse> Handle(GetAllCategories request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Started handling {nameof(GetAllCategoriesHandler)}");

        (IList<Category> categories, int totalRecordsCount) paginatedCategoriesResult =
            await GetPaginatedCategories(request, cancellationToken);
        return new GetAllCategoriesResponse(new GetAllCategoriesSuccess(),
            CreatePaginatedResult(paginatedCategoriesResult.categories, paginatedCategoriesResult.totalRecordsCount));
    }

    private async Task<(IList<Category> categories, int totalRecordsCount)> GetPaginatedCategories(GetAllCategories request,
        CancellationToken cancellationToken)
    {
        int totalRecordsCountTask = await _repository.GetCount(c => c.Id > 0);
        IList<Category> categoriesTask = await _repository.GetAll(request.LoadOptions, request.OrderByOption);

        //await Task.WhenAll(totalRecordsCountTask, categoriesTask);

        return new(categoriesTask, totalRecordsCountTask);
    }

    private IPaginatedResult<GetAllCategoriesResult> CreatePaginatedResult(IList<Category> categories,
        int totalRecordsCount)
    {
        List<GetAllCategoriesResult> getAllCategoriesResults = _mapper.Map<List<GetAllCategoriesResult>>(categories);
        return new PaginatedResult<GetAllCategoriesResult>(getAllCategoriesResults, totalRecordsCount);
    }
}