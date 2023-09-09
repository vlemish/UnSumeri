using AnSumeri.API.Domain.Contracts;
using MediatR;

namespace AnSumeri.API.Application.Articles.Events.ArticleDeleted;

public class DeleteArticleSearchRepositoryHandler : INotificationHandler<ArticleDeleted>
{
    private readonly IArticleSearchRepository _articleSearchRepository;

    public DeleteArticleSearchRepositoryHandler(IArticleSearchRepository articleSearchRepository)
    {
        _articleSearchRepository = articleSearchRepository;
    }

    public async Task Handle(ArticleDeleted notification, CancellationToken cancellationToken)
    {
        try
        {
            await _articleSearchRepository.RemoveAsync(notification.Id);
        }
        catch (Exception ex)
        {
            // log or republish?
        }
    }
}
