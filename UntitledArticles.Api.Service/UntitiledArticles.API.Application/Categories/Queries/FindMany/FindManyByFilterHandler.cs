using AutoMapper;
using MediatR;
using UntitiledArticles.API.Application.Categories.Queries.FindMany.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.FindOne;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitiledArticles.API.Application.OperationStatuses.Shared.Categories;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Queries.FindMany;

public class FindManyByFilterHandler : IRequestHandler<FindManyByFilter, ResultDto<FindManyByFilterResult>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public FindManyByFilterHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto<FindManyByFilterResult>> Handle(FindManyByFilter request, CancellationToken cancellationToken)
    {
        var filterResult = await _categoryRepository.GetManyByFilter(request.FilterExpression);
        if (filterResult is null)
        {
            return ReportNoContent();
        }

        FindManyByFilterResult result = new() { Categories = _mapper.Map<List<FindOneByFilterResult>>(filterResult) };
        return ReportSuccess(result);
    }

    private ResultDto<FindManyByFilterResult> ReportNoContent() =>
        new(new CategoryNoContent(), null);

    private ResultDto<FindManyByFilterResult> ReportSuccess(FindManyByFilterResult result) =>
        new(new FindManyByFilterSuccess(), result);
}
