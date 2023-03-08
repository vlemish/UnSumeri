using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.Add.Statuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Add
{
    public class AddCategoryHandler : IRequestHandler<AddCategory, AddCategoryResponse>
    {
        private readonly ILogger<AddCategoryHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public AddCategoryHandler(ILogger<AddCategoryHandler> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<AddCategoryResponse> Handle(AddCategory request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Try to create a new category where name = {request.Name}");
            Category category = CreateCategory(request.Name);
            Category addedCategory = await _categoryRepository.AddAsync(category);
            return ReportSuccess(addedCategory);
        }

        private AddCategoryResponse ReportSuccess(Category category)
        {
            _logger.LogDebug($"Add Category was successfully handled! Category Id = {category.Id}");
            return new(new AddCategorySuccess(category), new AddCategoryResult(category.Id));
        }

        private Category CreateCategory(string name) =>
            new() { Name = name };
    }
}
