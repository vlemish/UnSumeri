using MediatR;
using Microsoft.Extensions.Logging;
using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory.Statuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    using System.Linq.Expressions;
    using System.Xml.Linq;
    using Models.Mediatr;
    using OperationStatuses;
    using OperationStatuses.Shared.Categories;
    using Queries.FindOne;

    public class AddSubcategoryHandler : IRequestHandler<AddSubcategory, ResultDto<AddSubcategoryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;

        public AddSubcategoryHandler(ICategoryRepository categoryRepository, IMediator mediator)
        {
            _categoryRepository = categoryRepository;
            _mediator = mediator;
        }

        public async Task<ResultDto<AddSubcategoryResult>> Handle(AddSubcategory request,
            CancellationToken cancellationToken)
        {
            ResultDto<FindOneByFilterResult> findCategoryResult =
                await this._mediator.Send(new FindOneByFilter(CreateCategoryFilter(request)), cancellationToken);
            if (findCategoryResult.OperationStatus.Status == OperationStatusValue.OK)
            {
                return ReportConflict(request.Name);
            }

            Category createdCategory = await _categoryRepository.AddAsync(CreateCategory(request));
            return ReportSuccess(request, CreateSubcategoryResult(createdCategory));
        }

        private ResultDto<AddSubcategoryResult> ReportConflict(string name) =>
            new(new DuplicateCategory(name), null);

        private ResultDto<AddSubcategoryResult> ReportSuccess(AddSubcategory request, AddSubcategoryResult result) =>
            new(new AddSubcategorySuccess(request.Name, request.ParentId), result);

        private Expression<Func<Category, bool>> CreateCategoryFilter(AddSubcategory request) =>
            c => c.UserId == request.UserId && c.ParentId == request.ParentId && c.Name == request.Name;

        private Category CreateCategory(AddSubcategory request) =>
            new() { Name = request.Name, ParentId = request.ParentId, UserId = request.UserId };

        private AddSubcategoryResult CreateSubcategoryResult(Category category) =>
            new(category.Id);
    }
}
