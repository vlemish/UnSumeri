using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Queries.Statuses;

using UntitledArticles.API.Domain.Contracts;

namespace UntitiledArticles.API.Application.Categories.Queries
{
    public class GetCategoryHandler : IRequestHandler<GetCategory, GetCategoryResponse>
    {
        private readonly ILogger<GetCategoryHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryHandler(ILogger<GetCategoryHandler> logger, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<GetCategoryResponse> Handle(GetCategory request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetOneByFilter(p => p.Id == request.Id);
            if (category is null)
            {
               return ReportNotFound(request);
            }

            var result = _mapper.Map<GetCategoryResult>(category);

            return ReportSuccess(request, result);
        }

        private GetCategoryResponse ReportNotFound(GetCategory request)
        {
            _logger.LogDebug($"Failed to get a category where Id = {request.Id}: Category not found");
            return new GetCategoryResponse(new GetCategoryNotFound(request.Id), null);
        }


        private GetCategoryResponse ReportSuccess(GetCategory request, GetCategoryResult result)
        {
            _logger.LogDebug($"Get Category where Id = {request.Id} was successfully handled");
            return new GetCategoryResponse(new GetCategorySuccess(request.Id), result);
        }
    }
}
