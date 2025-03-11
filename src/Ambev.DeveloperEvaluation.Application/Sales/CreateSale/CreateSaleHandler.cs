using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM.UoW;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler responsible for processing the sale creation request.
/// </summary>
public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IDiscountService _discountService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IDiscountService discountService,
        IUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    /// <summary>
    /// Handles the sale creation process.
    /// </summary>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var sale = new Sale(
                saleNumber: GenerateSaleNumber(),
                saleDate: request.SaleDate,
                customerId: request.CustomerId,
                branchId: request.BranchId,
                items: request.Items.Select(item => new SaleItem(
                    saleId: Guid.NewGuid(),
                    productId: item.ProductId,
                    unitPrice: item.UnitPrice,
                    quantity: item.Quantity,
                    discount: _discountService.CalculateDiscount(item.Quantity, item.UnitPrice)
                )).ToList()
            );

            await _saleRepository.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync();

            // Publish the sale created event
            var saleCreatedEvent = new SaleCreatedEvent(sale.Id, sale.SaleNumber, sale.SaleDate);
            await _eventBus.PublishAsync(saleCreatedEvent);

            await _unitOfWork.CommitTransactionAsync();

            return new CreateSaleResult
            {
                SaleId = sale.Id,
                TotalAmount = sale.TotalAmount
            };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private static string GenerateSaleNumber()
    {
        return $"SALE-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
