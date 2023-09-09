using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Search;
using AnSumeri.API.Infrastructure.ElasticSearch.Constants;
using AnSumeri.API.Infrastructure.ElasticSearch.Factories;
using AnSumeri.API.Infrastructure.ElasticSearch.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch;

public static class ElasticSearchBootstrapper
{
    public static IServiceCollection ConfigureElasticSearch(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IElasticClient, ElasticClient>(provider =>
        {
            var settings = new ConnectionSettings(new Uri("http://172.18.0.2:9200"))
                .DefaultIndex("my_index");
            return new ElasticClient(settings);
        });

        services.AddScoped<IArticleSearchRepository, ArticleElasticSearchRepository>();
        services
            .AddTransient<IElasticSearchQueryFactory<ArticleSearchDto, ArticleSearchFilter>,
                ElasticSearchArticleQueryFactory>();
        services
            .AddTransient<IQueryContainerProvider<ArticleSearchDto, ArticleSearchFilter>,
                ArticleQueryContainerProvider>();

        return services;
    }
}
