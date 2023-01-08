using Core.Log.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace Core.Log.Extensions
{
    /// <summary>
    /// Contains extensions related to Serilog Logger Bootstraping
    /// </summary>
    public static class SerilogBootstraperExtensions
    {
        /// <summary>
        /// Configures Serilog Logging
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            Serilog.Log.Logger = CreateLogger(configuration);

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }

        private static Serilog.Core.Logger CreateLogger(IConfiguration configuration)
        {
            var serilogConfiguration = ParseSerilogConfiguration(configuration);
            var loggerBuilder = new LoggerConfiguration();

            if (serilogConfiguration.EnableConsoleLogging)
            {
                loggerBuilder.WriteTo.Console();
            }
            if (serilogConfiguration.EnableFileLogging)
            {
                loggerBuilder.WriteTo.File(serilogConfiguration.FilePath, serilogConfiguration.MinimumLogLevel);
            }

            return loggerBuilder.CreateLogger();
        }

        private static SerilogConfiguration ParseSerilogConfiguration(IConfiguration configuration)
        {
            SerilogConfiguration serilogConfiguration = new();
            configuration.GetSection("SerilogLogging").Bind(serilogConfiguration);
            return serilogConfiguration;
        }
    }
}
