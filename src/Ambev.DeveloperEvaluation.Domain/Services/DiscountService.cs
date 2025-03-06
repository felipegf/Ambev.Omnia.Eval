namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Implements the discount calculation logic based on business rules.
/// </summary>
public class DiscountService : IDiscountService
{
    public decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity >= 10 && quantity <= 20)
            return 0.2m * unitPrice * quantity;

        if (quantity >= 4)
            return 0.1m * unitPrice * quantity;

        return 0m;
    }
}
