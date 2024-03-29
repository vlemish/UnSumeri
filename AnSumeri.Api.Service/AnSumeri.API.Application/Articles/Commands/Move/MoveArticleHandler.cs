namespace AnSumeri.API.Application.Articles.Commands.Move;

using Categories.Queries.GetById;
using MediatR;
using Models;
using Models.Mediatr;
using OperationStatuses;
using OperationStatuses.Shared.Articles;
using Queries.GetOneById;
using Statuses;
using Domain.Contracts;
using Domain.Entities;

public class MoveArticleHandler : IRequestHandler<MoveArticle, ResultDto>
{
    private readonly IMediator _mediator;
    private readonly IArticleRepository _articleRepository;

    public MoveArticleHandler(IMediator mediator, IArticleRepository articleRepository)
    {
        _mediator = mediator;
        _articleRepository = articleRepository;
    }

    public async Task<ResultDto> Handle(MoveArticle request, CancellationToken cancellationToken)
    {
        ResultDto<ArticleDto> getArticleByIdResult =
            await _mediator.Send(CreateGetArticleByIdRequest(request.Id, request.UserId), cancellationToken);
        (bool isSuccess, IOperationStatus operationStatus) articleValidationResult =
            ValidateArticle(request, getArticleByIdResult);
        if (!articleValidationResult.isSuccess)
        {
            return new(articleValidationResult.operationStatus);
        }

        ResultDto<GetCategoryByIdResult> getCategoryByIdResult =
            await _mediator.Send(CreateGetCategoryByIdRequest(request.CategoryToMoveId), cancellationToken);
        (bool isSuccess, IOperationStatus operationStatus) categoryValidationResult =
            ValidateCategory(request, getCategoryByIdResult);
        if (!categoryValidationResult.isSuccess)
        {
            return new(categoryValidationResult.operationStatus);
        }

        await _articleRepository.UpdateAsync(CreateArticleForUpdate(getArticleByIdResult.Payload,
                request.CategoryToMoveId));
        return ReportSuccess(request);
    }

    private Article CreateArticleForUpdate(ArticleDto articleDto, int moveToCategoryId) =>
        new()
        {
            Id = articleDto.Id,
            CategoryId = moveToCategoryId,
            CreatedAtTime = articleDto.CreatedAtTime,
            Title = articleDto.Title,
            Content = articleDto.Content,
        };

    private (bool isSuccess, IOperationStatus operationStatus) ValidateArticle(MoveArticle request,
        ResultDto<ArticleDto> articleResult)
    {
        if (articleResult.OperationStatus.Status != OperationStatusValue.OK)
        {
            return new(false, articleResult.OperationStatus);
        }

        if (request.CategoryToMoveId == articleResult.Payload.CategoryId)
        {
            return new(false, new ArticleNotChanged(request.Id));
        }

        return new(true, null);
    }

    private (bool isSuccess, IOperationStatus operationStatus) ValidateCategory(MoveArticle request,
        ResultDto<GetCategoryByIdResult> categoryResult)
    {
        if (categoryResult.OperationStatus.Status != OperationStatusValue.OK)
        {
            return new(false, categoryResult.OperationStatus);
        }

        return new(true, null);
    }

    private ResultDto ReportSuccess(MoveArticle request) =>
        new(new MoveArticleSuccess(request.Id, request.CategoryToMoveId));

    private GetCategoryById CreateGetCategoryByIdRequest(int categoryId) =>
        new(categoryId, null, 1);

    private GetOneArticleById CreateGetArticleByIdRequest(int id, string userId) =>
        new(id, userId);
}
