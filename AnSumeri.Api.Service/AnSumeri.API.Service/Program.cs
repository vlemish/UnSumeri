using Serilog;

using AnSumeri.API.Service;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // entry point for configs
    builder.ConfigureServices();

    WebApplication? app = builder.Build();

    // entry point for app config
    app.ConfigureApplication();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Application was shut down");

    Log.CloseAndFlush();
}