using MediatR;

namespace AnSumeri.API.Application.Articles.Commands.Add;

using Models.Mediatr;

public record AddArticle(int CategoryId, string UserId, string Title, string Content) : IRequest<ResultDto<int>>;
