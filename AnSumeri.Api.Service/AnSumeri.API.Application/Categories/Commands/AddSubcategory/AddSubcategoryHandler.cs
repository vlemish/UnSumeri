using AnSumeri.API.Application.Categories.Commands.AddSubcategory.Statuses;
using MediatR;
using Microsoft.Extensions.Logging;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Categories.Commands.AddSubcategory
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
        private readonly IDateTimeProvider _dateTimeProvider;

        public AddSubcategoryHandler(ICategoryRepository categoryRepository, IMediator mediator, IDateTimeProvider dateTimeProvider)
        {
            _categoryRepository = categoryRepository;
            _mediator = mediator;
            _dateTimeProvider = dateTimeProvider;
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
            new() { Name = request.Name, ParentId = request.ParentId, UserId = request.UserId, CreatedAtTime = _dateTimeProvider.Current };

        private AddSubcategoryResult CreateSubcategoryResult(Category category) =>
            new(category.Id);
    }
}
