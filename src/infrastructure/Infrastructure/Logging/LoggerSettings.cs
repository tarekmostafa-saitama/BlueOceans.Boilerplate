namespace Infrastructure.Logging;

public class LoggerSettings
{
    public string AppName { get; set; }
    public bool WriteToFile { get; set; }
    public bool StructuredConsoleLogging { get; set; }
    public string MinimumLogLevel { get; set; } = "Information";
}