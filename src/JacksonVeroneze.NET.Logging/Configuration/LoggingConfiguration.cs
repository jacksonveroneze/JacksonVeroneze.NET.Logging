namespace JacksonVeroneze.NET.Logging.Configuration;

public class LoggingConfiguration
{
    public string? ApplicationName { get; set; }

    public string? ApplicationVersion { get; set; }

    public LogConfigurationConsole? Console { get; set; }

    public LogConfigurationSplunk? Splunk { get; set; }
}