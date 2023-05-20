using MediatR;

namespace UntitiledArticles.API.Application.Articles.Commands.Add;

public record AddArticle(int CategoryId, string Title, string Content) : IRequest<AddArticleResponse>;