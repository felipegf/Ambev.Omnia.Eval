namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Response containing sale details.
/// </summary>
public class GetSaleResponse
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The sale date.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The customer ID.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The branch ID.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// The total sale amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The items included in the sale.
    /// </summary>
    public List<GetSaleItemResponse> Items { get; set; } = new();
}

/// <summary>
/// Response containing sale item details.
/// </summary>
public class GetSaleItemResponse
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The discount applied to the product.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// The total amount for this item.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
