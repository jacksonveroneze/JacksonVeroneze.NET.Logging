using Serilog;
using Serilog.Events;

namespace JacksonVeroneze.NET.Logging.Util;

public class BootstrapLogger
{
    public static ILogger CreateLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }
}