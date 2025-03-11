namespace Ambev.DeveloperEvaluation.Common.Events;

/// <summary>
/// Defines a contract for domain events.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    DateTime OccurredOn { get; }

    /// <summary>
    /// Gets the ID of the aggregate root associated with this event.
    /// </summary>
    Guid AggregateId { get; }
}
