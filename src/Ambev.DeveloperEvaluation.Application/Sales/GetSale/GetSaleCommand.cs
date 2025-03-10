using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command to retrieve a sale.
/// </summary>
public class GetSaleCommand : IRequest<GetSaleResult>
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleCommand"/> class.
    /// </summary>
    /// <param name="saleId">The sale ID.</param>
    public GetSaleCommand(Guid saleId)
    {
        SaleId = saleId;
    }
}
