using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services;

/// <summary>
/// Unit tests for DiscountService.
/// </summary>
public class DiscountServiceTests
{
    private readonly DiscountService _discountService;

    /// <summary>
    /// Initializes a new instance of <see cref="DiscountServiceTests"/>.
    /// </summary>
    public DiscountServiceTests()
    {
        _discountService = new DiscountService();
    }

    /// <summary>
    /// Ensures that no discount is applied for purchases of fewer than 4 items.
    /// </summary>
    [Fact]
    public void CalculateDiscount_ShouldReturnZero_WhenQuantityIsLessThanFour()
    {
        // Arrange
        var unitPrice = 10m;
        var quantity = 3;

        // Act
        var discount = _discountService.CalculateDiscount(quantity, unitPrice);

        // Assert
        discount.Should().Be(0);
    }

    /// <summary>
    /// Ensures that a 10% discount is applied for purchases of 4 to 9 identical items.
    /// </summary>
    [Fact]
    public void CalculateDiscount_ShouldApply10PercentDiscount_WhenQuantityIsBetween4And9()
    {
        // Arrange
        var unitPrice = 10m;
        var quantity = 5;

        // Act
        var discount = _discountService.CalculateDiscount(quantity, unitPrice);

        // Assert
        discount.Should().Be(10m * 5 * 0.10m);
    }

    /// <summary>
    /// Ensures that a 20% discount is applied for purchases of 10 to 20 identical items.
    /// </summary>
    [Fact]
    public void CalculateDiscount_ShouldApply20PercentDiscount_WhenQuantityIsBetween10And20()
    {
        // Arrange
        var unitPrice = 15m;
        var quantity = 12;

        // Act
        var discount = _discountService.CalculateDiscount(quantity, unitPrice);

        // Assert
        discount.Should().Be(15m * 12 * 0.20m);
    }

    /// <summary>
    /// Ensures that an exception is thrown if the quantity exceeds 20.
    /// </summary>
    [Fact]
    public void CalculateDiscount_ShouldThrowException_WhenQuantityExceeds20()
    {
        // Arrange
        var unitPrice = 20m;
        var quantity = 21;

        // Act
        var act = () => _discountService.CalculateDiscount(quantity, unitPrice);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity cannot exceed 20 items.");
    }
}
