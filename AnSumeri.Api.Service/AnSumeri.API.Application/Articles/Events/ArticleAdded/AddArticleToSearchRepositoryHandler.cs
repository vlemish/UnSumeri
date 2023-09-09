using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Search;
using MediatR;

namespace AnSumeri.API.Application.Articles.Events.ArticleAdded;

public class AddArticleToSearchRepositoryHandler : INotificationHandler<ArticleAdded>
{
    private readonly IArticleSearchRepository _articleSearchRepository;

    public AddArticleToSearchRepositoryHandler(IArticleSearchRepository articleSearchRepository)
    {
        _articleSearchRepository = articleSearchRepository;
    }

    public async Task Handle(ArticleAdded notification, CancellationToken cancellationToken)
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
