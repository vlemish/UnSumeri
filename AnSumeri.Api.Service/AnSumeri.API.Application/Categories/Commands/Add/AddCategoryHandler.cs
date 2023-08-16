using MediatR;
using Microsoft.Extensions.Logging;
using AnSumeri.API.Application.Categories.Commands.Add.Statuses;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Categories.Commands.Add
{
    using System.Linq.Expressions;
    using Models.Mediatr;
    using OperationStatuses;
    using OperationStatuses.Shared.Categories;
    using Queries;
    using Queries.FindOne;

    public class AddCategoryHandler : IRequestHandler<AddCategory, ResultDto<AddCategoryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;

        public AddCategoryHandler(ICategoryRepository categoryRepository, IMediator mediator)
        {
            _mediator = mediator;
            _categoryRepository = categoryRepository;
        }

        public async Task<ResultDto<AddCategoryResult>> Handle(AddCategory request, CancellationToken cancellationToken)
        {
            ResultDto<FindOneByFilterResult> categoryFindResult =
                await this._mediator.Send(new FindOneByFilter(CreateCategoryFilter(request)));

            if (categoryFindResult.OperationStatus.Status == OperationStatusValue.OK)
            {
                return new ResultDto<AddCategoryResult>(new DuplicateCategory(request.Name), null);
            }

            Category addedCategory = await _categoryRepository.AddAsync(CreateCategory(request.Name, request.UserId));
            return ReportSuccess(addedCategory);
        }

        private Expression<Func<Category, bool>> CreateCategoryFilter(AddCategory request) =>
            c => c.UserId == request.UserId && c.Name == request.Name;

        private ResultDto<AddCategoryResult> ReportSuccess(Category category) =>
            new(new Statuses.AddCategorySuccess(category), new AddCategoryResult(category.Id));

        private Category CreateCategory(string name, string userId) =>
            new() { Name = name, UserId = userId };
    }
}
