namespace Ambev.DeveloperEvaluation.Common.Events;

/// <summary>
/// Defines an event bus for publishing and handling events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    /// <param name="event">The event instance.</param>
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

    /// <summary>
    /// Registers an event handler for a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <param name="handler">The event handler to register.</param>
    void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent;
}
