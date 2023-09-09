using MediatR;

namespace AnSumeri.API.Application.Articles.Events.ArticleAdded;

public record ArticleAdded(Guid UserId, int Id, string Title, string Content) : INotification;
