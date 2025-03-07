namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// Request to delete a sale.
/// </summary>
public class DeleteSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to delete.
    /// </summary>
    public Guid Id { get; set; }
}
