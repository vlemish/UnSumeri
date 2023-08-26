using AnSumeri.API.Application.Articles.Commands.Add.Statuses;
using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Application.OperationStatuses;

using MediatR;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Articles.Commands.Add;

using Models.Mediatr;
using OperationStatuses.Shared.Categories;

public class AddArticleHandler : IRequestHandler<AddArticle, ResultDto<AddArticleResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddArticleHandler(IArticleRepository articleRepository, IMediator mediator, IDateTimeProvider dateTimeProvider)
    {
        _articleRepository = articleRepository;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ResultDto<AddArticleResult>> Handle(AddArticle request, CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> getCategoryByIdResponse =
            await _mediator.Send(CreateGetCategoryById(request), cancellationToken);

        var validationResult = ValidateCategory(request, getCategoryByIdResponse);
        if (!validationResult.Success)
        {
            return new (GetFailureOperationStatus(request, validationResult.Status), null);
        }

        int addedArticleId = await AddCategoryAsync(request);
        return ReportSuccess(addedArticleId);
    }

    private async Task<int> AddCategoryAsync(AddArticle request)
    {
        Article article = CreateArticle(request);
        Article addedArticle = await _articleRepository.AddAsync(article);
        return addedArticle.Id;
    }


    private IOperationStatus GetFailureOperationStatus(AddArticle request, OperationStatusValue operationStatusValue)
    {
        switch (operationStatusValue)
        {
            case OperationStatusValue.NotFound:
                return new CategoryNotFound(request.CategoryId);
            case OperationStatusValue.Duplicate:
                return new AddArticleArticleAlreadyExist();
            default:
                throw new ArgumentOutOfRangeException(nameof(operationStatusValue));
        }
    }

    private (bool Success, OperationStatusValue Status) ValidateCategory(AddArticle request, ResultDto<GetCategoryByIdResult> response)
    {
        if (response.OperationStatus.Status is OperationStatusValue.NotFound)
        {
            return new(false, response.OperationStatus.Status);
        }

        bool articleExist = response.Payload.Articles.Any(a => a.Title == request.Title);
        return articleExist
        ? new(false, OperationStatusValue.Duplicate)
        : new(true, OperationStatusValue.OK);
    }

    private ResultDto<AddArticleResult> ReportSuccess(int addedArticleId) =>
        new(new AddArticleSuccessStatus(addedArticleId), new AddArticleResult(addedArticleId));

    private GetCategoryById CreateGetCategoryById(AddArticle request) =>
        new(request.CategoryId, null);

    private Article CreateArticle(AddArticle request) =>
        new()
        {
            CategoryId = request.CategoryId,
            Title = request.Title,
            Content = request.Content,
            UserId = request.UserId,
            CreatedAtTime = _dateTimeProvider.Current
        };
}
