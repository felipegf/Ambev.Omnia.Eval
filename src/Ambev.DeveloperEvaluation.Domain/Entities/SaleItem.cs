using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale transaction.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the ID of the associated sale.
    /// </summary>
    public Guid SaleId { get; private set; }

    /// <summary>
    /// Gets the product ID.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; }

    /// <summary>
    /// Gets the quantity of the product sold.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the discount applied to this item.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total amount for this item.
    /// </summary>
    public decimal TotalAmount => (UnitPrice * Quantity) - Discount;

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem(Guid saleId, Guid productId, decimal unitPrice, int quantity, decimal discount)
    {
        SaleId = saleId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount; // Agora o desconto é atribuído apenas no construtor
    }

    /// <summary>
    /// Updates the quantity of the item.
    /// </summary>
    /// <param name="quantity">The new quantity.</param>
    public void UpdateQuantity(int quantity)
    {
        if (quantity < 1 || quantity > 20)
            throw new ArgumentException("Quantity must be between 1 and 20.");

        Quantity = quantity;
    }

    /// <summary>
    /// Validates the SaleItem entity using the SaleItemValidator rules.
    /// </summary>
    /// <returns>A <see cref="ValidationResultDetail"/> containing validation results.</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail(result);
    }
}
