namespace Ambev.DeveloperEvaluation.Common.Events;

/// <summary>
/// Represents a base class for domain events.
/// </summary>
public abstract class EventBase : IEvent
{
    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the ID of the aggregate root associated with this event.
    /// </summary>
    public Guid AggregateId { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventBase"/> class.
    /// </summary>
    /// <param name="aggregateId">The ID of the aggregate root.</param>
    protected EventBase(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
}
