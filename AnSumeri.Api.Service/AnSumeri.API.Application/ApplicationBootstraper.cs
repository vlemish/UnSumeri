using FluentValidation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using AnSumeri.API.Application.Models.Factories;
using AnSumeri.API.Application.PipelineBehaviours;

namespace AnSumeri.API.Application
{
    public static class ApplicationBootstraper
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICategoryMoveStrategyFactory, CategoryMoveStrategyFactory>();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.ConfigurePipelineBehaviors(configuration);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }

        private static IServiceCollection ConfigurePipelineBehaviors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
