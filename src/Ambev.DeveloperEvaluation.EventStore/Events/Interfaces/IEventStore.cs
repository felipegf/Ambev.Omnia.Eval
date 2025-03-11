using Ambev.DeveloperEvaluation.Common.Events;

namespace Ambev.DeveloperEvaluation.EventLog.Events.Interfaces;

/// <summary>
/// Defines an event store responsible for persisting domain events.
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// Saves an event in the event store.
    /// </summary>
    /// <param name="event">The event to be stored.</param>
    Task SaveEventAsync(IEvent @event);

    /// <summary>
    /// Retrieves all events related to a specific aggregate.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>A list of events.</returns>
    Task<IEnumerable<IEvent>> GetEventsByAggregateIdAsync(Guid aggregateId);
}
