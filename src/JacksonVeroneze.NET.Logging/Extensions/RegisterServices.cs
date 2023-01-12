using System.Diagnostics.CodeAnalysis;
using JacksonVeroneze.NET.Logging.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace JacksonVeroneze.NET.Logging.Extensions;

[ExcludeFromCodeCoverage]
public static class RegisterServices
{
    public static IHostBuilder AddLogging(
        this IHostBuilder host,
        Action<LoggingConfiguration> action)
    {
        host.UseSerilog((hostingContext, _, loggerConfiguration) =>
        {
            LoggingConfiguration optionsConfig = new();

            action.Invoke(optionsConfig);

            loggerConfiguration.ReadFrom.Configuration(
                    hostingContext.Configuration)
                .ConfigureLogger(optionsConfig);
        });

        return host;
    }

    private static LoggerConfiguration ConfigureLogger(
        this LoggerConfiguration loggerConfiguration,
        LoggingConfiguration optionsConfig)
    {
        loggerConfiguration.ConfigureEnrich(optionsConfig);

        if (optionsConfig.Console?.IsEnable ?? false)
        {
            loggerConfiguration.WriteConsole();
        }

        if (optionsConfig.Splunk?.IsEnable ?? false)
        {
            loggerConfiguration.WriteSplunk(optionsConfig);
        }

        return loggerConfiguration;
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

    private static LoggerConfiguration WriteConsole(
        this LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .WriteTo.Async(write =>
                write.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj} " +
                    "{Properties:j}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate));

        return loggerConfiguration;
    }

    private static LoggerConfiguration WriteSplunk(
        this LoggerConfiguration loggerConfiguration,
        LoggingConfiguration optionsConfig)
    {
        loggerConfiguration
            .WriteTo.EventCollector(optionsConfig.Splunk!.Host, optionsConfig.Splunk.Token,
                index: optionsConfig.Splunk.Index);

        return loggerConfiguration;
    }
}