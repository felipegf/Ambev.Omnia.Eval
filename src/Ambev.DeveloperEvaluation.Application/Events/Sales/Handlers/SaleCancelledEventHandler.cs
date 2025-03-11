using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.Application.Events.Sales;

/// <summary>
/// Handles the SaleCancelledEvent.
/// </summary>
public class SaleCancelledEventHandler : IEventHandler<SaleCancelledEvent>
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IEventBus _eventBus;
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    public SaleCancelledEventHandler(IMongoDatabase mongoDatabase, IEventBus eventBus, ILogger<SaleCancelledEventHandler> logger)
    {
        _mongoDatabase = mongoDatabase;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task HandleAsync(SaleCancelledEvent @event)
    {
        _logger.LogInformation("Persisting SaleCancelledEvent for Sale ID {SaleId} into Event Log.", @event.AggregateId);

        var collection = _mongoDatabase.GetCollection<IEvent>("EventLogs");
        await collection.InsertOneAsync(@event);

        // Ensure the event is published
        await _eventBus.PublishAsync(@event);
    }

}

