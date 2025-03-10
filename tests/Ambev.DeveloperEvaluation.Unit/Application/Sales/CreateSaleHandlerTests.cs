namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM.UoW;
using FluentValidation;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// Unit tests for the CreateSaleHandler.
/// Ensures that the sale creation process works correctly.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<IDiscountService> _discountServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes the test dependencies using Moq.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _discountServiceMock = new Mock<IDiscountService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateSaleHandler(
            _saleRepositoryMock.Object,
            _discountServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    /// <summary>
    /// Tests if a sale is successfully created and persisted in the repository.
    /// </summary>
    [Fact]
    public async Task Handle_Should_Create_Sale_Successfully()
    {
        // Arrange
        var validSale = new Sale(
            saleNumber: "SALE-TEST",
            saleDate: DateTime.UtcNow,
            customerId: Guid.NewGuid(),
            branchId: Guid.NewGuid(),
            items: new List<SaleItem>
            {
            new SaleItem(
                saleId: Guid.NewGuid(),
                productId: Guid.NewGuid(),
                unitPrice: 10.0m,
                quantity: 2,
                discount: 0
            )
            }
        );

        var command = new CreateSaleCommand
        {
            CustomerId = validSale.CustomerId,
            BranchId = validSale.BranchId,
            SaleDate = validSale.SaleDate,
            Items = validSale.SaleItems.Select(item => new CreateSaleItemCommand
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount
            }).ToList()
        };

        _saleRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Sale>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        _saleRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Sale>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Tests if an exception is thrown when an invalid sale is provided.
    /// </summary>
    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Sale_Is_Invalid()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.Empty,
            SaleDate = DateTime.UtcNow.AddDays(1),
            Items = []
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests if the UnitOfWork is not committed when an error occurs.
    /// </summary>
    [Fact]
    public async Task Handle_Should_Not_Commit_When_Error_Occurs()
    {
        // Arrange
        var validSale = new Sale(
            saleNumber: "SALE-TEST",
            saleDate: DateTime.UtcNow,
            customerId: Guid.NewGuid(),
            branchId: Guid.NewGuid(),
            items: new List<SaleItem>
            {
            new SaleItem(
                saleId: Guid.NewGuid(),
                productId: Guid.NewGuid(),
                unitPrice: 10.0m,
                quantity: 2,
                discount: 0
            )
            }
        );

        var command = new CreateSaleCommand
        {
            CustomerId = validSale.CustomerId,
            BranchId = validSale.BranchId,
            SaleDate = validSale.SaleDate,
            Items = validSale.SaleItems.Select(item => new CreateSaleItemCommand
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount
            }).ToList()
        };

        _saleRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Sale>()))
            .ThrowsAsync(new System.Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Database error", exception.Message);

        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

}
