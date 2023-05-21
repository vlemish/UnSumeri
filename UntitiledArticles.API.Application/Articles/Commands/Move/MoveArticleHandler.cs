namespace UntitiledArticles.API.Application.Articles.Commands.Move;

using AutoMapper;
using Categories.Queries.GetById;
using MediatR;
using Models;
using Models.Mediatr;
using OperationStatuses;
using Queries.GetOneById;
using Statuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

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
            await this._mediator.Send(CreateGetArticleByIdRequest(request.Id), cancellationToken);
        (bool isSuccess, IOperationStatus operationStatus) articleValidationResult =
            ValidateArticle(request, getArticleByIdResult);
        if (!articleValidationResult.isSuccess)
        {
            return new(articleValidationResult.operationStatus);
        }

        ResultDto<GetCategoryByIdResult> getCategoryByIdResult =
            await this._mediator.Send(CreateGetCategoryByIdRequest(request.CategoryToMoveId), cancellationToken);
        (bool isSuccess, IOperationStatus operationStatus) categoryValidationResult =
            this.ValidateCategory(request, getCategoryByIdResult);
        if (!categoryValidationResult.isSuccess)
        {
            return new(categoryValidationResult.operationStatus);
        }

        await this._articleRepository.UpdateAsync(CreateArticleForUpdate(getArticleByIdResult.Payload,
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
            return new(false, new MoveArticleNotChanged(request.Id, request.CategoryToMoveId));
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

    private ResultDto ReportNotModifiedStatus(MoveArticle request) =>
        new(new MoveArticleNotChanged(request.Id, request.CategoryToMoveId));

    private GetCategoryById CreateGetCategoryByIdRequest(int categoryId) =>
        new(categoryId, 1);

    private GetOneArticleById CreateGetArticleByIdRequest(int id) =>
        new(id);
}
