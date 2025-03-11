namespace Ambev.DeveloperEvaluation.EventLog.LogStorage.Entities;

/// <summary>
/// Represents a log entry stored in MongoDB.
/// </summary>
public class LogEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
}
