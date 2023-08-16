using AnSumeri.API.Application.Categories.Queries.GetAll.Statuses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;
using AnSumeri.API.Domain.Enums;
using AnSumeri.API.Domain.Pagination;

namespace AnSumeri.API.Application.Categories.Queries.GetAll;

using Models.Mediatr;

public class
    GetAllCategoriesHandler : IRequestHandler<GetAllCategories, ResultDto<IPaginatedResult<GetAllCategoriesResult>>>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(ICategoryRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto<IPaginatedResult<GetAllCategoriesResult>>> Handle(GetAllCategories request,
        CancellationToken cancellationToken)
    {
        (IList<Category> categories, int totalRecordsCount) paginatedCategoriesResult =
            await GetPaginatedCategories(request, cancellationToken);
        return new ResultDto<IPaginatedResult<GetAllCategoriesResult>>(new GetAllCategoriesSuccess(),
            CreatePaginatedResult(paginatedCategoriesResult.categories, paginatedCategoriesResult.totalRecordsCount));
    }

    private async Task<(IList<Category> categories, int totalRecordsCount)> GetPaginatedCategories(
        GetAllCategories request,
        CancellationToken cancellationToken)
    {
        int totalRecordsCount = await _repository.GetCount(c => c.UserId == request.UserId);
        if (totalRecordsCount < 1)
        {
            return new(Enumerable.Empty<Category>().ToList(), totalRecordsCount);
        }
        IList<Category> categories =
            await _repository.GetAll(request.LoadOptions, request.OrderByOption, c => c.UserId == request.UserId,
                request.Depth);

        return new(categories, totalRecordsCount);
    }

    private IPaginatedResult<GetAllCategoriesResult> CreatePaginatedResult(IList<Category> categories,
        int totalRecordsCount)
    {
        List<GetAllCategoriesResult> getAllCategoriesResults = _mapper.Map<List<GetAllCategoriesResult>>(categories);
        return new PaginatedResult<GetAllCategoriesResult>(getAllCategoriesResults, totalRecordsCount);
    }
}
