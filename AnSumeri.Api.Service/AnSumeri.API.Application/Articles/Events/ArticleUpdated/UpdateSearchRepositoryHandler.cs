using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Search;
using MediatR;

namespace AnSumeri.API.Application.Articles.Events.ArticleUpdated;

public class UpdateSearchRepositoryHandler : INotificationHandler<ArticleUpdated>
{
    private readonly IArticleSearchRepository _articleSearchRepository;

    public UpdateSearchRepositoryHandler(IArticleSearchRepository articleSearchRepository)
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
