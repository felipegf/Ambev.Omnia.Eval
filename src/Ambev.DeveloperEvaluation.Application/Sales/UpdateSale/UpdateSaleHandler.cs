using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.UoW;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handles the process of updating a sale.
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandler"/> class.
    /// </summary>
    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEventBus eventBus,
        ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the update of a sale.
    /// </summary>
    public async Task<bool> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);
        if (sale is null)
        {
            _logger.LogWarning("Sale with ID {SaleId} not found.", request.SaleId);
            return false;
        }

        _mapper.Map(request, sale);
        _saleRepository.Update(sale);

        await _unitOfWork.SaveChangesAsync();

        // Publish SaleUpdatedEvent
        var saleUpdatedEvent = new SaleUpdatedEvent(sale.Id, sale.SaleNumber, sale.SaleDate);
        await _eventBus.PublishAsync(saleUpdatedEvent);

        _logger.LogInformation("SaleUpdatedEvent published for Sale ID {SaleId}.", sale.Id);

        return true;
    }
}
