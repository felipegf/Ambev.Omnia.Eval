namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service responsible for calculating discounts based on quantity tiers.
/// </summary>
public class DiscountService : IDiscountService
{
    /// <inheritdoc />
    public decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new ArgumentException("Quantity cannot exceed 20 items.");

        if (quantity >= 10)
            return unitPrice * quantity * 0.20m; // 20% discount

        if (quantity >= 4)
            return unitPrice * quantity * 0.10m; // 10% discount

        return 0m; // No discount
    }
}
