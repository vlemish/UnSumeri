﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Infrastructure.Repositories;

namespace AnSumeri.API.Infrastructure
{
    public static class InfrastructureBoostraper
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServerDocker")));
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // var db = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            // db.Database.Migrate();

            return services;
        }
    }
}
