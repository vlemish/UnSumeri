using Authentication.Service;

using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices();

    var app = builder.Build();

    app.ConfigureApplication();

    app.Run();
}
catch(Exception ex)
{
    Log.Error($"There was a problem setting up the application: {ex.Message}", ex);

    throw;
}
finally
{
    Log.Information("The application was shut down!");
}