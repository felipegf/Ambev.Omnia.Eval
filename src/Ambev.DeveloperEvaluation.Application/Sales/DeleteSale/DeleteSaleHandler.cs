using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.UoW;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handles the deletion of a sale (soft delete).
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandler"/> class.
    /// </summary>
    public DeleteSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    /// <summary>
    /// Handles the sale deletion process (soft delete).
    /// </summary>
    public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);
        if (sale is null)
        {
            return false;
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            _saleRepository.Delete(sale);
            await _unitOfWork.SaveChangesAsync();

            // Publish the sale cancelled event
            var saleCancelledEvent = new SaleCancelledEvent(sale.Id, "Sale cancelled.");
            await _eventBus.PublishAsync(saleCancelledEvent);

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
