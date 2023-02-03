using MediatR;

namespace UntitledSelfArticles.API.Application.Articles.Commands.AddArticle
{
    public record AddArticle(string Title, string Context, int CategoryId) : IRequest<AddArticleResponse>
}
