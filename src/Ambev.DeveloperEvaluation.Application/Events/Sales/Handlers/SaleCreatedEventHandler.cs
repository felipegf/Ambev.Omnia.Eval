using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.Application.Events.Sales;

/// <summary>
/// Handles the <see cref="SaleCreatedEvent"/> and persists it into the event log.
/// </summary>
public class SaleCreatedEventHandler : IEventHandler<SaleCreatedEvent>
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IEventBus _eventBus;
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleCreatedEventHandler"/> class.
    /// </summary>
    /// <param name="mongoDatabase">The MongoDB database instance.</param>
    /// <param name="eventBus">The event bus.</param>
    /// <param name="logger">Logger instance.</param>
    public SaleCreatedEventHandler(IMongoDatabase mongoDatabase, IEventBus eventBus, ILogger<SaleCreatedEventHandler> logger)
    {
        _mongoDatabase = mongoDatabase;
        _eventBus = eventBus;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task HandleAsync(SaleCreatedEvent @event)
    {
        _logger.LogInformation("Persisting SaleCreatedEvent for Sale ID {SaleId} into Event Log.", @event.AggregateId);

        var collection = _mongoDatabase.GetCollection<IEvent>("EventLogs");
        await collection.InsertOneAsync(@event);

        // Ensure the event is published
        await _eventBus.PublishAsync(@event);
    }
}
