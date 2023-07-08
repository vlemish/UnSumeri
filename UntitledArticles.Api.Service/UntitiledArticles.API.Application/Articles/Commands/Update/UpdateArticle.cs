namespace UntitiledArticles.API.Application.Articles.Commands.Update;

using MediatR;
using Models.Mediatr;

public record UpdateArticle(int Id, string Title, string Content) : IRequest<ResultDto>;
