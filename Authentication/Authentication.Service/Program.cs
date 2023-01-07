using Authentication.Service;

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
    // log exception

    throw;
}
finally
{
    // log that application has shut down
}