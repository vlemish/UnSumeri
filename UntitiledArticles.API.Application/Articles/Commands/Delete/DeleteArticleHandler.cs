namespace UntitiledArticles.API.Application.Articles.Commands.Delete;

using MediatR;
using Models;
using Models.Mediatr;
using OperationStatuses;
using Queries.GetOneById;
using Statuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

public class DeleteArticleHandler : IRequestHandler<DeleteArticle, ResultDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMediator _mediator;

    public DeleteArticleHandler(IArticleRepository articleRepository, IMediator mediator)
    {
        _articleRepository = articleRepository;
        _mediator = mediator;
    }

    public async Task<ResultDto> Handle(DeleteArticle request, CancellationToken cancellationToken)
    {
        ResultDto<ArticleDto> articleResultDto =
            await this._mediator.Send(new GetOneArticleById(request.Id), cancellationToken);
        if (articleResultDto.OperationStatus.Status != OperationStatusValue.OK)
        {
            return new(articleResultDto.OperationStatus);
        }

        await this._articleRepository.DeleteAsync(CreateArticle(articleResultDto));
        return ReportSuccess(request.Id);
    }

    private ResultDto ReportSuccess(int id) =>
        new ResultDto(new DeleteArticleSuccess(id));

    private Article CreateArticle(ResultDto<ArticleDto> resultDto) =>
        new() { Id = resultDto.Payload.Id };
}
