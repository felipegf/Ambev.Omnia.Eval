namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Defines a service for calculating discounts based on quantity.
/// </summary>
public interface IDiscountService
{
    /// <summary>
    /// Calculates the discount amount based on business rules.
    /// </summary>
    /// <param name="quantity">The number of items purchased.</param>
    /// <param name="unitPrice">The price per unit.</param>
    /// <returns>The discount amount.</returns>
    decimal CalculateDiscount(int quantity, decimal unitPrice);
}
