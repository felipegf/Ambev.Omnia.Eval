using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.UoW;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handles the update of a sale.
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">The repository for handling sales operations.</param>
    /// <param name="unitOfWork">Unit of Work for transaction management.</param>
    /// <param name="mapper">Mapper for transforming DTOs into entities.</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the update of a sale.
    /// </summary>
    /// <param name="request">The command containing sale details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if the sale was updated successfully, otherwise false.</returns>
    public async Task<bool> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);
        if (sale is null)
        {
            return false;
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {            
            var updatedSale = new Sale(
                saleNumber: sale.SaleNumber,
                saleDate: request.SaleDate,
                customerId: sale.CustomerId,
                branchId: sale.BranchId,
                items: request.Items.Select(i => new SaleItem(
                    saleId: sale.Id,
                    productId: i.ProductId,
                    unitPrice: i.UnitPrice,
                    quantity: i.Quantity,
                    discount: i.Discount
                )).ToList()
            );

            _saleRepository.Update(updatedSale);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

}
