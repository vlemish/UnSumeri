using AutoMapper;

using MediatR;

using UntitiledArticles.API.Application.Articles.Commands.Add.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Articles.Commands.Add;

using Models.Mediatr;

public class AddArticleHandler : IRequestHandler<AddArticle, AddArticleResponse>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AddArticleHandler(IArticleRepository articleRepository, IMapper mapper, IMediator mediator)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AddArticleResponse> Handle(AddArticle request, CancellationToken cancellationToken)
    {
        ResultDto<GetCategoryByIdResult> getCategoryByIdResponse =
            await _mediator.Send(CreateGetCategoryById(request), cancellationToken);

        var validationResult = ValidateCategory(request, getCategoryByIdResponse);
        if (!validationResult.Success)
        {
            return new AddArticleResponse(GetFailureOperationStatus(request, validationResult.Status), null);
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
                return new AddArticleCategoryNotExist(request.CategoryId);
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

    private AddArticleResponse ReportSuccess(int addedArticleId) =>
        new(new AddArticleSuccessStatus(addedArticleId), new AddArticleResult(addedArticleId));

    private GetCategoryById CreateGetCategoryById(AddArticle request) =>
        new(request.CategoryId);

    private Article CreateArticle(AddArticle request) =>
        new()
        {
            CategoryId = request.CategoryId,
            Title = request.Title,
            Content = request.Content,
            CreatedAtTime = DateTime.UtcNow
        };
}
