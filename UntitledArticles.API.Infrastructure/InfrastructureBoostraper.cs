using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Infrastructure.Repositories;

namespace UntitledArticles.API.Infrastructure
{
    public static class InfrastructureBoostraper
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServerDocker")));
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        } 
    }
}
