using AutoMapper;

using Serilog;

using System.Reflection;

using UntitiledArticles.API.Application;

using UntitledArticles.API.Infrastructure;
using UntitledArticles.API.Service.Mappings;
using UntitledArticles.API.Service.Middlewares;

namespace UntitledArticles.API.Service
{
    public static class StartupConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            //Log.Logger = CreateLogger();
            builder.Services.ConfigureSerilogLogger(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureInfrastructure(builder.Configuration);
            builder.Services.ConfigureApplication(builder.Configuration);

            builder.Services.AddAutoMapper(configAction => configAction.AddProfiles(new List<Profile>()
            {
                new CategoryMappings(),
            }));

            return builder;
        }

        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }

        private static IServiceCollection ConfigureSerilogLogger(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = CreateLogger();
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            return services;
        }

        private static Serilog.Core.Logger CreateLogger() =>
            new LoggerConfiguration()
            .ReadFrom.Configuration(GetLoggerConfiguration())
            .CreateLogger();

        private static IConfigurationRoot? GetLoggerConfiguration() =>
            new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
               .Build();
    }
}
