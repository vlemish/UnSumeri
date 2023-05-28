namespace UntitiledArticles.API.Application.Articles.Commands.Delete;

using MediatR;
using Models.Mediatr;

public record DeleteArticle(int Id) : IRequest<ResultDto>;
