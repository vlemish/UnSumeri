using MediatR;

namespace UntitiledArticles.API.Application.Articles.Commands.Add;

using Models.Mediatr;

public record AddArticle(int CategoryId, string Title, string Content) : IRequest<ResultDto<AddArticleResult>>;
