using FluentValidation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using UntitiledArticles.API.Application.Categories.Commands.Add;
using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory;
using UntitiledArticles.API.Application.Categories.Commands.Move;
using UntitiledArticles.API.Application.Models.Factories;

namespace UntitiledArticles.API.Application
{
    public static class ApplicationBoostraper
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICategoryMoveStrategyFactory, CategoryMoveStrategyFactory>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddDbContextFactory()
            services.AddScoped<IValidator<AddCategory>, AddCategoryValidator>();
            services.AddScoped<IValidator<AddSubcategory>, AddSubcategoryValidator>();
            services.AddScoped<IValidator<MoveCategory>, MoveCategoryValidator>();
            return services;
        }
    }
}
