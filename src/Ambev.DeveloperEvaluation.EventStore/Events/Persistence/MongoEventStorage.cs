using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.EventLog.Events.Interfaces;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.EventLog.Events.Persistence;

/// <summary>
/// MongoDB implementation of <see cref="IEventStore"/> for storing domain events.
/// </summary>
public class MongoEventStorage : IEventStore
{
    private readonly NoSqlContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoEventStorage"/> class.
    /// </summary>
    /// <param name="noSqlContext">The NoSQL database context.</param>
    public MongoEventStorage(NoSqlContext noSqlContext)
    {
        _context = noSqlContext;
    }

    /// <inheritdoc />
    public async Task SaveEventAsync(IEvent @event)
    {
        await _context.EventLogs.InsertOneAsync(@event);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IEvent>> GetEventsByAggregateIdAsync(Guid aggregateId)
    {
        return await _context.EventLogs
            .Find(e => e.AggregateId == aggregateId)
            .ToListAsync();
    }
}
