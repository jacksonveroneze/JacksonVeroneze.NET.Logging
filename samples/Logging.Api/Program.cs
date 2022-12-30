using JacksonVeroneze.NET.Logging.Configuration;
using JacksonVeroneze.NET.Logging.Extensions;

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