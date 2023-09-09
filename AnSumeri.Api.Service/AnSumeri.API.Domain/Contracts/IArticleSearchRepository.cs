using System.Collections.Immutable;
using AnSumeri.API.Domain.Search;

namespace AnSumeri.API.Domain.Contracts;

public interface IArticleSearchRepository
{
    Task AddAsync(ArticleSearchDto articleSearchDto);

    Task AddManyAsync(IEnumerable<ArticleSearchDto> articleSearchList);

    Task RemoveAsync(int id);

    Task RemoveManyAsync(IEnumerable<int> identifiers);

    Task<ArticleSearchDto> FindOne(ArticleSearchFilter filter);

    Task<IImmutableList<ArticleSearchDto>> Find(ArticleSearchFilter filter);
}
