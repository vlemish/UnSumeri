using AutoMapper;

using MediatR;

using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Application.Categories.Queries.GetById.Statuses;
using AnSumeri.API.Application.OperationStatuses.Shared.Categories;

namespace AnSumeri.API.Application.Categories.Queries.GetById
{
    using Models.Mediatr;

    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryById, ResultDto<GetCategoryByIdResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ResultDto<GetCategoryByIdResult>> Handle(GetCategoryById request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetOneByFilter(c=> c.Id == request.Id && c.UserId == request.UserId, request.Depth);
            if (category is null)
            {
                return ReportNotFound(request);
            }

            var result = _mapper.Map<GetCategoryByIdResult>(category);
            return ReportSuccess(request, result);
        }

        private ResultDto<GetCategoryByIdResult> ReportNotFound(GetCategoryById request) =>
            new(new CategoryNotFound(request.Id), null);

        private ResultDto<GetCategoryByIdResult>  ReportSuccess(GetCategoryById request, GetCategoryByIdResult result) =>
            new(new GetCategoryByIdSuccess(request.Id), result);
    }
}
