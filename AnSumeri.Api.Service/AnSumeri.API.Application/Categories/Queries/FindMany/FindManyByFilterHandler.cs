using AnSumeri.API.Application.Categories.Queries.FindMany.Statuses;
using AnSumeri.API.Application.Categories.Queries.FindOne;
using AnSumeri.API.Application.Models.Mediatr;
using AnSumeri.API.Application.OperationStatuses.Shared.Categories;
using AutoMapper;
using MediatR;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Categories.Queries.FindMany;

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
