using Ambev.DeveloperEvaluation.EventLog.LogStorage.Entities;
using Ambev.DeveloperEvaluation.EventLog.LogStorage.Interfaces;

namespace Ambev.DeveloperEvaluation.EventLog.LogStorage.Persistence;

/// <summary>
/// MongoDB implementation of <see cref="ILogStorage"/>.
/// </summary>
public class MongoLogStorage : ILogStorage
{
    private readonly NoSqlContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoLogStorage"/> class.
    /// </summary>
    /// <param name="noSqlContext">The NoSQL database context.</param>
    public MongoLogStorage(NoSqlContext noSqlContext)
    {
        _context = noSqlContext;
    }

    /// <inheritdoc />
    public async Task StoreLogAsync(LogEntry logEntry)
    {
        await _context.ApplicationLogs.InsertOneAsync(logEntry);
    }
}
