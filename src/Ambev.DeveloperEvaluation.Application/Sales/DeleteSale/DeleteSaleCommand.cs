using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Represents a request to delete a sale.
/// </summary>
public class DeleteSaleCommand : IRequest<bool>
{
    /// <summary>
    /// Gets the sale ID to be deleted.
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleCommand"/> class.
    /// </summary>
    /// <param name="saleId">The sale ID to be deleted.</param>
    public DeleteSaleCommand(Guid saleId)
    {
        SaleId = saleId;
    }
}
