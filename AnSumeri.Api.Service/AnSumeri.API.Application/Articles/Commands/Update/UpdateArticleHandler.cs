namespace AnSumeri.API.Application.Articles.Commands.Update;

using MediatR;
using Models;
using Models.Mediatr;
using OperationStatuses;
using OperationStatuses.Shared.Articles;
using Queries.GetOneById;
using Statuses;
using Domain.Contracts;
using Domain.Entities;

public class UpdateArticleHandler : IRequestHandler<UpdateArticle, ResultDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMediator _mediator;

    public UpdateArticleHandler(IMediator mediator, IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
        _mediator = mediator;
    }

    public async Task<ResultDto> Handle(UpdateArticle request, CancellationToken cancellationToken)
    {
        ResultDto<ArticleDto> getOneArticleByIdResult =
            await this._mediator.Send(new GetOneArticleById(request.Id, request.UserId), cancellationToken);
        if (getOneArticleByIdResult.OperationStatus.Status != OperationStatusValue.OK)
        {
            return new ResultDto(getOneArticleByIdResult.OperationStatus);
        }

        if (!IsUpdatesAnyOfFields(getOneArticleByIdResult.Payload, request))
        {
            return ReportNotModified(request);
        }

        await this._articleRepository.UpdateAsync(CreateArticleToUpdate(request, getOneArticleByIdResult.Payload));
        return ReportSuccess(request);
    }

    private bool IsUpdatesAnyOfFields(ArticleDto articleDto, UpdateArticle updateArticle) =>
        articleDto.Content != updateArticle.Content ||
        articleDto.Title != updateArticle.Title;

    private ResultDto ReportNotModified(UpdateArticle requset) =>
        new(new ArticleNotChanged(requset.Id));

    private ResultDto ReportSuccess(UpdateArticle request) =>
        new(new UpdateArticleSuccess(request.Id));

    private Article CreateArticleToUpdate(UpdateArticle request, ArticleDto articleDto) =>
        new()
        {
            Id = request.Id, CategoryId = articleDto.CategoryId, Title = request.Title, Content = request.Content, CreatedAtTime = articleDto.CreatedAtTime,
        };
}
