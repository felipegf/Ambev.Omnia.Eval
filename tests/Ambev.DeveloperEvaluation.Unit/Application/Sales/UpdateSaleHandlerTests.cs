namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.UoW;
using AutoMapper;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Unit tests for the UpdateSaleHandler.
/// Ensures that the sale update process works correctly.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<ILogger<UpdateSaleHandler>> _loggerMock;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _eventBusMock = new Mock<IEventBus>();
        _loggerMock = new Mock<ILogger<UpdateSaleHandler>>();

        _handler = new UpdateSaleHandler(
            _saleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _eventBusMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Update_Sale_Successfully()
    {
        var sale = new Sale("SALE-123", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid(), []);
        var command = new UpdateSaleCommand { SaleId = sale.Id, SaleDate = sale.SaleDate };

        _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(sale.Id)).ReturnsAsync(sale);
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
        _eventBusMock.Setup(bus => bus.PublishAsync(It.IsAny<SaleUpdatedEvent>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<SaleUpdatedEvent>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_If_Sale_Not_Found()
    {
        var command = new UpdateSaleCommand { SaleId = Guid.NewGuid(), SaleDate = DateTime.UtcNow };

        _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Sale)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<SaleUpdatedEvent>()), Times.Never);
    }
}
