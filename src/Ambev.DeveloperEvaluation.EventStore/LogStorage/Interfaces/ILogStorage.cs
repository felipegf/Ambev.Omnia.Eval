using Ambev.DeveloperEvaluation.EventLog.LogStorage.Entities;

namespace Ambev.DeveloperEvaluation.EventLog.LogStorage.Interfaces;

/// <summary>
/// Interface for storing logs in a centralized log storage.
/// </summary>
public interface ILogStorage
{
    /// <summary>
    /// Stores a log entry asynchronously.
    /// </summary>
    /// <param name="logEntry">The log entry to store.</param>
    Task StoreLogAsync(LogEntry logEntry);
}
