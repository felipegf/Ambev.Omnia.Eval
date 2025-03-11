using Ambev.DeveloperEvaluation.Application.Events.Sales;
using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Events;

/// <summary>
/// Unit tests for the SaleCancelledEventHandler.
/// Ensures that sale cancellation events are handled correctly.
/// </summary>
public class SaleCancelledEventHandlerTests
{
    private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
    private readonly Mock<IMongoCollection<IEvent>> _eventCollectionMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<ILogger<SaleCancelledEventHandler>> _loggerMock;
    private readonly SaleCancelledEventHandler _handler;

    public SaleCancelledEventHandlerTests()
    {
        _mongoDatabaseMock = new Mock<IMongoDatabase>();
        _eventCollectionMock = new Mock<IMongoCollection<IEvent>>();
        _eventBusMock = new Mock<IEventBus>();
        _loggerMock = new Mock<ILogger<SaleCancelledEventHandler>>();

        _mongoDatabaseMock
            .Setup(db => db.GetCollection<IEvent>("EventLogs", null))
            .Returns(_eventCollectionMock.Object);

        _handler = new SaleCancelledEventHandler(_mongoDatabaseMock.Object, _eventBusMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Log_And_Persist_Event()
    {
        // Arrange
        var saleCancelledEvent = new SaleCancelledEvent(Guid.NewGuid(), "Customer Requested Refund");

        // Act
        await _handler.HandleAsync(saleCancelledEvent);

        // Assert
        _loggerMock.Verify(log => log.Log(
            It.Is<LogLevel>(level => level == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Persisting SaleCancelledEvent")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);

        _eventCollectionMock.Verify(coll => coll.InsertOneAsync(saleCancelledEvent, null, default), Times.Once);

        // Ensure PublishAsync() is called
        _eventBusMock.Verify(bus => bus.PublishAsync(saleCancelledEvent), Times.Once);
    }

}

