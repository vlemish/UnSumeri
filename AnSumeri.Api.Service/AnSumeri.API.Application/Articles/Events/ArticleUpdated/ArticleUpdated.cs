using MediatR;

namespace AnSumeri.API.Application.Articles.Events.ArticleUpdated;

public record ArticleUpdated(Guid UserId, int Id, string Title, string Content) : INotification;
