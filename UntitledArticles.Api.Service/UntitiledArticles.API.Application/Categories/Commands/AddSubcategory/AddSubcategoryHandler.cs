using MediatR;
using Microsoft.Extensions.Logging;
using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory.Statuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    using Models.Mediatr;

    public class AddSubcategoryHandler : IRequestHandler<AddSubcategory, ResultDto<AddSubcategoryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public AddSubcategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResultDto<AddSubcategoryResult>> Handle(AddSubcategory request,
            CancellationToken cancellationToken)
        {
            Category createdCategory = await _categoryRepository.AddAsync(CreateCategory(request));
            return new ResultDto<AddSubcategoryResult>(new AddSubcategorySuccess(request.Name, request.ParentId),
                CreateSubcategoryResult(createdCategory));
        }

        private Category CreateCategory(AddSubcategory request) =>
            new() { Name = request.Name, ParentId = request.ParentId, UserId = request.UserId };

        private AddSubcategoryResult CreateSubcategoryResult(Category category) =>
            new(category.Id);
    }
}
