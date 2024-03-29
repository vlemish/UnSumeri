using System.Collections.Immutable;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Constants;
using AnSumeri.API.Infrastructure.ElasticSearch.Factories;
using AnSumeri.API.Infrastructure.ElasticSearch.Providers;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch;

public class ArticleElasticSearchRepository : IArticleSearchRepository
{
    private readonly IElasticClient _client;
    private readonly IQueryContainerProvider<ArticleSearchDto, ArticleAllShouldMustFilter> _queryContainerProvider;
    private readonly IElasticSearchQueryDirectorFactory _queryDirectorFactory;

    public ArticleElasticSearchRepository(IElasticClient client,
        IQueryContainerProvider<ArticleSearchDto, ArticleAllShouldMustFilter> queryContainerProvider, IElasticSearchQueryDirectorFactory queryDirectorFactory)
    {
        _queryContainerProvider = queryContainerProvider;
        _client = client;
        _queryDirectorFactory = queryDirectorFactory;
        DefineIndexes();
    }

    public async Task AddAsync(ArticleSearchDto articleSearchDto)
    {
        var indexResponse =
            await _client.IndexAsync(articleSearchDto, descriptor => descriptor.Id(articleSearchDto.Id));
        if (!indexResponse.IsValid)
        {
            // TODO: Create appropriate exceptions
            throw new ArgumentException(indexResponse.DebugInformation);
        }
    }

    public Task AddManyAsync(IEnumerable<ArticleSearchDto> articleSearchList)
    {
        return _client.BulkAsync(b => b
            .IndexMany(articleSearchList,
                (descriptor, document) => descriptor
                    .Index(ElasticSearchIndexConstants.ArticleIndex)
                    .Id(document.Id)
                    .Document(document)));
    }

    public Task RemoveAsync(int id)
    {
         return _client.DeleteAsync<ArticleSearchDto>(id);
    }

    public async Task RemoveManyAsync(IEnumerable<int> identifiers)
    {
        await _client.BulkAsync(b => b.DeleteMany<ArticleSearchDto>(identifiers.Select(s => s.ToString()),
            (descriptor, id) =>
                descriptor.Index(ElasticSearchIndexConstants.ArticleIndex)
                    .Id(id)));
    }

    public async Task<ArticleSearchDto> FindOne(IArticleSearchFilter filter)
    {
        var searchResponse = await _client.SearchAsync<ArticleSearchDto>(s =>
            s.Query(q => _queryDirectorFactory.CreateQueryDirector(filter.SearchMode, q).Direct(filter))
                .Size(1));

        return searchResponse.Hits.Select(s => s.Source).FirstOrDefault();
    }

    public async Task<IImmutableList<ArticleSearchDto>> Find(IArticleSearchFilter filter)
    {
        var searchResponse = await _client.SearchAsync<ArticleSearchDto>(s =>
            s.Query(q => _queryDirectorFactory.CreateQueryDirector(filter.SearchMode, q).Direct(filter)));
        return searchResponse.Hits.Select(s => s.Source).ToImmutableList();
    }

    private void DefineIndexes()
    {
        _client.Indices.Create(ElasticSearchIndexConstants.ArticleIndex, c => c.Map<ArticleSearchDto>(m => m.AutoMap()
            .Properties(p => p.Text(t => t
                .Name(x => x.UserId)
                .Analyzer("standard")))
            .Properties(p => p.Text(t => t
                    .Name(x => x.Title)
                    .Analyzer("standard")
                )
                .Text(t => t
                    .Name(x => x.Content)
                    .Analyzer("standard")))));
    }
}
