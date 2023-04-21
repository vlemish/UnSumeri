using FluentValidation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.PipelineBehaviours;

namespace UntitiledArticles.API.Application
{
    public static class ApplicationBoostraper
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICategoryMoveStrategyFactory, CategoryMoveStrategyFactory>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddDbContextFactory()

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
