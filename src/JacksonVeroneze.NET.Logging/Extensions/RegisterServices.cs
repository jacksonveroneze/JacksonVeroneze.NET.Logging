using System.Diagnostics.CodeAnalysis;
using JacksonVeroneze.NET.Logging.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

namespace JacksonVeroneze.NET.Logging.Extensions;

[ExcludeFromCodeCoverage]
public static class RegisterServices
{
    public static IHostBuilder AddLogging(
        this IHostBuilder host,
        Action<LoggingConfiguration> action)
    {
        host.UseSerilog((hostingContext, services, loggerConfiguration) =>
        {
            LoggingConfiguration optionsConfig = new();

            action.Invoke(optionsConfig);

            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .ReadFrom.Services(services)
                .ConfigureEnrich(optionsConfig);
        });

        return host;
    }

    private static LoggerConfiguration ConfigureEnrich(
        this LoggerConfiguration loggerConfiguration,
        LoggingConfiguration optionsConfig)
    {
        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails()
            .Enrich.WithEnvironmentName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithCorrelationIdHeader()
            .Enrich.WithSpan()
            .Enrich.WithProperty("ApplicationName", optionsConfig.ApplicationName)
            .Enrich.WithProperty("ApplicationVersion", optionsConfig.ApplicationVersion);

        return loggerConfiguration;
    }
}