using AnSumeri.API.Application.Articles.Events.ArticleDeleted;

namespace AnSumeri.API.Application.Articles.Commands.Delete;

using MediatR;
using Models;
using Models.Mediatr;
using OperationStatuses;
using Queries.GetOneById;
using Statuses;
using Domain.Contracts;
using Domain.Entities;

public class DeleteArticleHandler : IRequestHandler<DeleteArticle, ResultDto<int>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMediator _mediator;

    public DeleteArticleHandler(IArticleRepository articleRepository, IMediator mediator)
    {
        _articleRepository = articleRepository;
        _mediator = mediator;
    }

    public async Task<ResultDto<int>> Handle(DeleteArticle request, CancellationToken cancellationToken)
    {
        ResultDto<ArticleDto> articleResultDto =
            await this._mediator.Send(new GetOneArticleById(request.Id, request.UserId), cancellationToken);
        if (articleResultDto.OperationStatus.Status != OperationStatusValue.OK)
        {
            return new(articleResultDto.OperationStatus, 0);
        }

        await _articleRepository.DeleteAsync(CreateArticle(articleResultDto));
        await PublishEvent(request.Id);
        return ReportSuccess(request.Id);
    }

    private ResultDto<int> ReportSuccess(int id) =>
        new(new DeleteArticleSuccess(id), id);

    private Task PublishEvent(int id) =>
        _mediator.Publish(new ArticleDeleted(id));

    private Article CreateArticle(ResultDto<ArticleDto> resultDto) =>
        new()
        {
            Id = resultDto.Payload.Id,
            Title = resultDto.Payload.Title,
            Content = resultDto.Payload.Content,
            CategoryId = resultDto.Payload.CategoryId,
            CreatedAtTime = resultDto.Payload.CreatedAtTime,
        };
}
