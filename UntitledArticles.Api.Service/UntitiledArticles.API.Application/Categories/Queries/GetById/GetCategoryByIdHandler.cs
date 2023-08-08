using System.Linq.Expressions;
using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Queries.GetById.Statuses;

using UntitledArticles.API.Domain.Contracts;

namespace UntitiledArticles.API.Application.Categories.Queries.GetById
{
    using Models.Mediatr;

    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryById, ResultDto<GetCategoryByIdResult>>
    {
        private readonly ILogger<GetCategoryByIdHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(ILogger<GetCategoryByIdHandler> logger, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _logger = logger;
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

        private ResultDto<GetCategoryByIdResult> ReportNotFound(GetCategoryById request)
        {
            _logger.LogDebug($"Failed to get a category where Id = {request.Id}: Category not found");
            return new ResultDto<GetCategoryByIdResult>(new GetCategoryByIdNotFound(request.Id), null);
        }


        private ResultDto<GetCategoryByIdResult>  ReportSuccess(GetCategoryById request, GetCategoryByIdResult result)
        {
            _logger.LogDebug($"Get Category where Id = {request.Id} was successfully handled");
            return new ResultDto<GetCategoryByIdResult>(new GetCategoryByIdSuccess(request.Id), result);
        }
    }
}
