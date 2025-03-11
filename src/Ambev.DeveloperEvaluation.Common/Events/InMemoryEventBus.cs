using System.Collections.Concurrent;

namespace Ambev.DeveloperEvaluation.Common.Events;

/// <summary>
/// Implements an in-memory event bus for handling domain events.
/// </summary>
public sealed class InMemoryEventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<Func<IEvent, Task>>> _handlers = new();

    /// <inheritdoc/>
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (_handlers.TryGetValue(typeof(TEvent), out var handlers))
        {
            foreach (var handler in handlers)
            {
                try
                {
                    await handler(@event);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EventBus] Error handling event {typeof(TEvent).Name}: {ex.Message}");
                }
            }
        }
    }

    /// <inheritdoc/>
    public void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.ContainsKey(eventType))
        {
            _handlers[eventType] = new List<Func<IEvent, Task>>();
        }

        _handlers[eventType].Add(@event => handler((TEvent)@event));
    }

    /// <summary>
    /// Checks if there are subscribers for a given event type.
    /// </summary>
    public bool HasSubscribers<TEvent>() where TEvent : IEvent
    {
        return _handlers.ContainsKey(typeof(TEvent)) && _handlers[typeof(TEvent)].Count > 0;
    }
}
