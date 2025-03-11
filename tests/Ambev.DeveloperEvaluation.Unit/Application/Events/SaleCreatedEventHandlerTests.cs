using Ambev.DeveloperEvaluation.Application.Events.Sales;
using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Events;

/// <summary>
/// Unit tests for the SaleCreatedEventHandler.
/// Ensures that sale creation events are processed correctly.
/// </summary>
public class SaleCreatedEventHandlerTests
{
    private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
    private readonly Mock<IMongoCollection<IEvent>> _eventCollectionMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<ILogger<SaleCreatedEventHandler>> _loggerMock;
    private readonly SaleCreatedEventHandler _handler;

    public SaleCreatedEventHandlerTests()
    {
        _mongoDatabaseMock = new Mock<IMongoDatabase>();
        _eventCollectionMock = new Mock<IMongoCollection<IEvent>>();
        _eventBusMock = new Mock<IEventBus>();
        _loggerMock = new Mock<ILogger<SaleCreatedEventHandler>>();

        // Configurar para que o MongoDB retorne uma coleção válida
        _mongoDatabaseMock
            .Setup(db => db.GetCollection<IEvent>("EventLogs", null))
            .Returns(_eventCollectionMock.Object);

        _handler = new SaleCreatedEventHandler(_mongoDatabaseMock.Object, _eventBusMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Log_And_Persist_Event()
    {
        // Arrange
        var saleCreatedEvent = new SaleCreatedEvent(Guid.NewGuid(), "SALE123", DateTime.UtcNow);

        // Act
        await _handler.HandleAsync(saleCreatedEvent);

        // Assert
        _loggerMock.Verify(log => log.Log(
            It.Is<LogLevel>(level => level == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Persisting SaleCreatedEvent")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);

        _eventCollectionMock.Verify(coll => coll.InsertOneAsync(saleCreatedEvent, null, default), Times.Once);

        // Ensure PublishAsync() is called
        _eventBusMock.Verify(bus => bus.PublishAsync(saleCreatedEvent), Times.Once);
    }
}

