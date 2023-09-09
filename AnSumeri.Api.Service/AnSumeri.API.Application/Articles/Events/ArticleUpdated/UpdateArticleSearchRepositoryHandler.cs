using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Enums;
using MediatR;

namespace AnSumeri.API.Application.Articles.Events.ArticleUpdated;

public class UpdateArticleSearchRepositoryHandler : INotificationHandler<ArticleUpdated>
{
    private readonly IArticleSearchRepository _articleSearchRepository;

    public UpdateArticleSearchRepositoryHandler(IArticleSearchRepository articleSearchRepository)
    {
        _articleSearchRepository = articleSearchRepository;
    }

    public async Task Handle(ArticleUpdated notification, CancellationToken cancellationToken)
    {
        try
        {
            await _articleSearchRepository.AddAsync(new ArticleSearchDto(notification.Id, notification.UserId, notification.Title,
                notification.Content));
        }
        catch (Exception ex)
        {
            // log or republish?
        }
    }
}
