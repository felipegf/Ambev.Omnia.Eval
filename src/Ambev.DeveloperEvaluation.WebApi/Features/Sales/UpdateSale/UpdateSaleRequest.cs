using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Represents the request to update a sale.
/// </summary>
public class UpdateSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to update.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the sale date.
    /// </summary>
    [Required]
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the list of sale items.
    /// </summary>
    [Required]
    public List<UpdateSaleItemRequest> Items { get; set; } = [];
}

/// <summary>
/// Represents an item in the update sale request.
/// </summary>
public class UpdateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the updated quantity.
    /// </summary>
    [Required]
    [Range(1, 20, ErrorMessage = "Quantity must be between 1 and 20.")]
    public int Quantity { get; set; }
}
