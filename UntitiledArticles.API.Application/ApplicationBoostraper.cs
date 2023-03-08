using FluentValidation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using UntitiledArticles.API.Application.Categories.Commands.Add;

namespace UntitiledArticles.API.Application
{
    public static class ApplicationBoostraper
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddDbContextFactory()
            services.AddScoped<IValidator<AddCategory>, AddCategoryValidator>();
            return services;
        }
    }
}
