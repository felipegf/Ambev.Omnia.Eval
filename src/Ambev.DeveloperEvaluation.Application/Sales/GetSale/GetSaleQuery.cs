using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Query to retrieve a sale by its ID.
/// </summary>
public record GetSaleQuery(Guid SaleId) : IRequest<GetSaleResult>;
