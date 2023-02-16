using JacksonVeroneze.NET.Logging.Configuration;
using JacksonVeroneze.NET.Logging.Extensions;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Host.AddLogging(config =>
    {
        config.ApplicationName = "JacksonVeroneze.NET.Logging";
        config.ApplicationVersion = "1.0.0";
        config.Console = new LogConfigurationConsole
        {
            IsEnable = true
        };
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