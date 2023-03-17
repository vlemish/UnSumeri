using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory.Statuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    public class AddSubcategoryHandler : IRequestHandler<AddSubcategory, AddSubcategoryResponse>
    {
        private readonly ILogger<AddSubcategoryHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public AddSubcategoryHandler(ILogger<AddSubcategoryHandler> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<AddSubcategoryResponse> Handle(AddSubcategory request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Start handling {nameof(AddSubcategory)} request where Name = {request.Name}, Parent Id = {request.ParentId}");
            Category createdCategory = await _categoryRepository.AddAsync(CreateCategory(request));
            _logger.LogDebug($"{nameof(AddSubcategory)} request where Name = {request.Name}, Parent Id = {request.ParentId} was successfully handled");
            return new AddSubcategoryResponse(new AddSubcategorySuccess(request.Name, request.ParentId), CreateSubcategoryResult(createdCategory));
        }

        private Category CreateCategory(AddSubcategory request) =>
            new() { Name = request.Name, ParentId = request.ParentId };

        private AddSubcategoryResult CreateSubcategoryResult(Category category) =>
            new(category.Id);
    }
}
