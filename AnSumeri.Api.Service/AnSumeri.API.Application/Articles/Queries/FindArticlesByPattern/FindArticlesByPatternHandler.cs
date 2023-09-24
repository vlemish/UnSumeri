using System.Collections.Immutable;
using AnSumeri.API.Application.Articles.Queries.FindArticlesByPattern.Statuses;
using AnSumeri.API.Application.Extensions;
using AnSumeri.API.Application.Models;
using AnSumeri.API.Application.Models.Mediatr;
using AnSumeri.API.Application.OperationStatuses.Shared.Articles;
using AnSumeri.API.Application.OperationStatuses.Shared.Categories;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Enums;
using AnSumeri.API.Domain.Search.Filters;
using MediatR;

namespace AnSumeri.API.Application.Articles.Queries.FindArticlesByPattern;

public class FindArticlesByPatternHandler : IRequestHandler<FindArticlesByPattern,
    ResultDto<ImmutableList<FindArticlesByPatternResult>>>
{
    private readonly IArticleSearchRepository _searchRepository;

    public FindArticlesByPatternHandler(IArticleSearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }

    public async Task<ResultDto<ImmutableList<FindArticlesByPatternResult>>> Handle(FindArticlesByPattern request,
        CancellationToken cancellationToken)
    {
        IImmutableList<ArticleSearchDto> searchResult =
            await _searchRepository.Find(CreateArticleSearchFilterByPattern(request));
        if (searchResult is null || searchResult.Count == 0)
        {
            return new(new ArticleNoContent(), null);
        }

        ImmutableList<FindArticlesByPatternResult> findArticleByPatternResult =
            searchResult.Select(sr => sr.ToFindArticleByPatternResult()).ToImmutableList();
        return new(new FindArticlesByPatternSuccess(request.UserId, request.SearchPattern), findArticleByPatternResult);
    }

    private IArticleSearchFilter CreateArticleSearchFilterByPattern(FindArticlesByPattern request) =>
        new ArticleAcrossSearchFilter(request.SearchPattern, Guid.Parse(request.UserId), SearchMode.Across,
            a => a.Title, a => a.Content);
}
