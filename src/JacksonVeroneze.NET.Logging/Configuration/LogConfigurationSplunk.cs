namespace JacksonVeroneze.NET.Logging.Configuration;

public class LogConfigurationSplunk
{
    public bool IsEnable { get; set; }

    public string? Host { get; set; }

    public string? Token { get; set; }

    public string? Index { get; set; }
}