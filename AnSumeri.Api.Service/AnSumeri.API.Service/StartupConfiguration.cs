using AutoMapper;
using Serilog;
using System.Reflection;
using AnSumeri.API.Application;
using AnSumeri.API.Infrastructure;
using AnSumeri.API.Infrastructure.ElasticSearch;
using AnSumeri.API.Service.Mappings;
using AnSumeri.API.Service.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace AnSumeri.API.Service
{
    using System.Text;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;

    public static class StartupConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            //Log.Logger = CreateLogger();
            builder.Services.ConfigureSerilogLogger(builder.Configuration);

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = Assembly.GetExecutingAssembly().GetName().ToString(), Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
            });

            builder.Services.ConfigureInfrastructure(builder.Configuration);
            builder.Services.ConfigureApplication(builder.Configuration);
            builder.Services.ConfigureElasticSearch(builder.Configuration);

            builder.Services.AddAutoMapper(configAction =>
                configAction.AddProfiles(new List<Profile>() { new CategoryMappings(), }));

            return builder;
        }

        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            ApplyPendingMigrations(app);

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI();
            // }

            // app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private static IServiceCollection ConfigureSerilogLogger(this IServiceCollection services,
            IConfiguration configuration)
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
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    true)
                .Build();

        private static void ApplyPendingMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                // Here is the migration executed
                dbContext.Database.Migrate();
            }
        }
    }
}
