using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents the request to create a sale.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    [Required]
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    [Required]
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the sale date.
    /// </summary>
    [Required]
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the list of sale items.
    /// </summary>
    [Required]
    public List<CreateSaleItemRequest> Items { get; set; } = [];
}

/// <summary>
/// Represents an item in the sale request.
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    [Required]
    [Range(1, 20, ErrorMessage = "Quantity must be between 1 and 20.")]
    public int Quantity { get; set; }
}
