using AnSumeri.API.Application.Models.Mediatr;

namespace AnSumeri.API.Application.Categories.Queries.FindOne;

using AutoMapper;
using AutoMapper.Execution;
using MediatR;
using OperationStatuses.Shared.Categories;
using Statuses;
using Application.Categories.Queries.GetById;
using Application.Models.Mediatr;
using Domain.Contracts;
using Domain.Entities;

public class FindOneByFilterHandler : IRequestHandler<FindOneByFilter, ResultDto<FindOneByFilterResult>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public FindOneByFilterHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        this._categoryRepository = categoryRepository;
        this._mapper = mapper;
    }

    public async Task<ResultDto<FindOneByFilterResult>> Handle(FindOneByFilter request, CancellationToken cancellationToken)
    {
        Category category = await this._categoryRepository.GetOneByFilter(request.FilterExpression);
        if (category is null)
        {
            return this.ReportNoContent();
        }

        FindOneByFilterResult result = _mapper.Map<FindOneByFilterResult>(category);
        return ReportSuccess(result);
    }

    private ResultDto<FindOneByFilterResult> ReportNoContent() =>
        new(new CategoryNoContent(), null);

    private ResultDto<FindOneByFilterResult> ReportSuccess(FindOneByFilterResult result) =>
        new(new FindOneByFilterSuccess(), result);
}
