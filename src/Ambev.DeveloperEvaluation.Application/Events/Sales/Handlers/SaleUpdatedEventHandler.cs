using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.Application.Events.Sales;

/// <summary>
/// Handles the SaleUpdatedEvent.
/// </summary>
public class SaleUpdatedEventHandler : IEventHandler<SaleUpdatedEvent>
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IEventBus _eventBus;
    private readonly ILogger<SaleUpdatedEventHandler> _logger;

    public SaleUpdatedEventHandler(IMongoDatabase mongoDatabase, IEventBus eventBus, ILogger<SaleUpdatedEventHandler> logger)
    {
        _mongoDatabase = mongoDatabase;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task HandleAsync(SaleUpdatedEvent @event)
    {
        _logger.LogInformation("Persisting SaleUpdatedEvent for Sale ID {SaleId} into Event Log.", @event.AggregateId);

        var collection = _mongoDatabase.GetCollection<IEvent>("EventLogs");
        await collection.InsertOneAsync(@event);

        // Ensure the event is published
        await _eventBus.PublishAsync(@event);
    }

}

