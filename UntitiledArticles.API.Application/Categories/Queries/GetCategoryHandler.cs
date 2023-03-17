using MediatR;

using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

using UntitiledArticles.API.Application.Categories.Queries.Statuses;
using UntitiledArticles.API.Application.Models.Categories;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Queries
{
    public class GetCategoryHandler : IRequestHandler<GetCategory, GetCategoryResponse>
    {
        private readonly ILogger<GetCategoryHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryHandler(ILogger<GetCategoryHandler> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryResponse> Handle(GetCategory request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetOneByFilter(p => p.Id == request.Id);
            if (category is null)
            {
                ReportNotFound(request);
            }

            var result = CreateGetCategoryResult(category);

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

        private GetCategoryResult CreateGetCategoryResult(Category category)
        {
            var result = new GetCategoryResult()
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                SubCategories = CreateCategoryReadDtos(category.SubCategories),
            };

            return result;
        }

        private ReadOnlyCollection<CategoryReadDto> CreateCategoryReadDtos(ICollection<Category> categories)
        {
            if (categories is null)
            {
                return null;
            }
            List<CategoryReadDto> categoryReadDtos = new();
            foreach (var category in categories)
            {
                var subCategory = new CategoryReadDto()
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentId = category.ParentId,
                    SubCategories = CreateCategoryReadDtos(category.SubCategories),
                };
                categoryReadDtos.Add(subCategory);
            }

            return categoryReadDtos.AsReadOnly();
        }

    }
}
