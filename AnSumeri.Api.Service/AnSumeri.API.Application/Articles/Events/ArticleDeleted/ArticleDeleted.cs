using MediatR;

namespace AnSumeri.API.Application.Articles.Events.ArticleDeleted;

public record ArticleDeleted(int Id) : INotification;
