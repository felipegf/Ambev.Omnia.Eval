using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

/// <summary>
/// Query to retrieve all sales.
/// </summary>
public record GetAllSalesQuery() : IRequest<List<GetSaleResult>>;
