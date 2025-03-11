namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.UoW;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// Unit tests for the DeleteSaleHandler.
/// Ensures that the sale deletion process works correctly.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly DeleteSaleHandler _handler;

    public DeleteSaleHandlerTests()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _eventBusMock = new Mock<IEventBus>();

        _handler = new DeleteSaleHandler(
            _saleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _eventBusMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Delete_Sale_Successfully()
    {
        var sale = new Sale("SALE-123", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid(), []);
        var command = new DeleteSaleCommand(sale.Id);

        _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(sale.Id)).ReturnsAsync(sale);
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
        _eventBusMock.Setup(bus => bus.PublishAsync(It.IsAny<SaleCancelledEvent>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<SaleCancelledEvent>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_If_Sale_Not_Found()
    {
        var command = new DeleteSaleCommand(Guid.NewGuid());

        _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Sale)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<SaleCancelledEvent>()), Times.Never);
    }
}
