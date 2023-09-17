using System.Collections.Immutable;
using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Filters;

namespace AnSumeri.API.Domain.Contracts;

public interface IArticleSearchRepository
{
    Task AddAsync(ArticleSearchDto articleSearchDto);

    Task AddManyAsync(IEnumerable<ArticleSearchDto> articleSearchList);

    Task RemoveAsync(int id);

    Task RemoveManyAsync(IEnumerable<int> identifiers);

    Task<ArticleSearchDto> FindOne(IArticleSearchFilter filter);

    Task<IImmutableList<ArticleSearchDto>> Find(IArticleSearchFilter filter);
}
