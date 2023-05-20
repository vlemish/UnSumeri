using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Commands.Add.Statuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Add
{
    using Models.Mediatr;

    public class AddCategoryHandler : IRequestHandler<AddCategory, ResultDto<AddCategoryResult>>
    {
        private readonly ILogger<AddCategoryHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public AddCategoryHandler(ILogger<AddCategoryHandler> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<ResultDto<AddCategoryResult>> Handle(AddCategory request, CancellationToken cancellationToken)
        {
            Category addedCategory = await _categoryRepository.AddAsync(CreateCategory(request.Name));
            return ReportSuccess(addedCategory);
        }

        private ResultDto<AddCategoryResult> ReportSuccess(Category category) =>
            new(new Statuses.AddCategorySuccess(category), new AddCategoryResult(category.Id));

        private Category CreateCategory(string name) =>
            new() { Name = name };
    }
}
