using JacksonVeroneze.NET.Logging.Extensions;
using JacksonVeroneze.NET.Logging.Util;
using Serilog;

Log.Logger = BootstrapLogger.CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Host.AddLogging(config =>
    {
        config.ApplicationName = "JacksonVeroneze.NET.Logging";
        config.ApplicationVersion = "1.0.0";
    });

    var app = builder.Build();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}