namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents the response after creating a sale.
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
