using Ambev.DeveloperEvaluation.Application.Events.Sales;
using Ambev.DeveloperEvaluation.Common.Events;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Events;

/// <summary>
/// Unit tests for the SaleUpdatedEventHandler.
/// Ensures that sale update events are handled correctly.
/// </summary>
public class SaleUpdatedEventHandlerTests
{
    private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
    private readonly Mock<IMongoCollection<IEvent>> _eventCollectionMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<ILogger<SaleUpdatedEventHandler>> _loggerMock;
    private readonly SaleUpdatedEventHandler _handler;

    public SaleUpdatedEventHandlerTests()
    {
        _mongoDatabaseMock = new Mock<IMongoDatabase>();
        _eventCollectionMock = new Mock<IMongoCollection<IEvent>>();
        _eventBusMock = new Mock<IEventBus>();
        _loggerMock = new Mock<ILogger<SaleUpdatedEventHandler>>();

        _mongoDatabaseMock
            .Setup(db => db.GetCollection<IEvent>("EventLogs", null))
            .Returns(_eventCollectionMock.Object);

        _handler = new SaleUpdatedEventHandler(_mongoDatabaseMock.Object, _eventBusMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Log_And_Persist_Event()
    {
        // ✅ Correção: Passando três argumentos agora
        var saleUpdatedEvent = new SaleUpdatedEvent(Guid.NewGuid(), "UPDATED_SALE_001", DateTime.UtcNow);

        // Act
        await _handler.HandleAsync(saleUpdatedEvent);

        // Assert
        _loggerMock.Verify(log => log.Log(
            It.Is<LogLevel>(level => level == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Persisting SaleUpdatedEvent")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);

        _eventCollectionMock.Verify(coll => coll.InsertOneAsync(saleUpdatedEvent, null, default), Times.Once);

        // Ensure PublishAsync() is called
        _eventBusMock.Verify(bus => bus.PublishAsync(saleUpdatedEvent), Times.Once);
    }
}
