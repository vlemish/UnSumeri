namespace AnSumeri.API.Application.Articles.Queries.GetOneById;

using MediatR;
using Models;
using Models.Mediatr;

public record GetOneArticleById(int Id, string UserId) : IRequest<ResultDto<ArticleDto>>;
