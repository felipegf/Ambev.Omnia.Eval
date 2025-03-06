using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command to update an existing sale.
/// </summary>
public class UpdateSaleCommand : IRequest<bool>
{
    public Guid SaleId { get; set; }
    public DateTime SaleDate { get; set; }
    public List<UpdateSaleItemCommand> Items { get; set; } = new();
}

/// <summary>
/// Represents an item to be updated in a sale.
/// </summary>
public class UpdateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the discount applied to the item.
    /// </summary>
    public decimal Discount { get; set; } 
}
