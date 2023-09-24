using System.Collections.Immutable;
using AnSumeri.API.Application.Models;
using AnSumeri.API.Application.Models.Mediatr;
using MediatR;

namespace AnSumeri.API.Application.Articles.Queries.FindArticlesByPattern;

public record FindArticlesByPattern(string UserId, string SearchPattern) : IRequest<ResultDto<ImmutableList<FindArticlesByPatternResult>>>;
