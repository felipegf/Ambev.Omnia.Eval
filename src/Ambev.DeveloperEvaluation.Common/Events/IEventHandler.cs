namespace Ambev.DeveloperEvaluation.Common.Events;

/// <summary>
/// Defines a contract for handling domain events.
/// </summary>
/// <typeparam name="TEvent">The type of event to handle.</typeparam>
public interface IEventHandler<TEvent> where TEvent : IEvent
{
    /// <summary>
    /// Handles an event asynchronously.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    Task HandleAsync(TEvent @event);
}
