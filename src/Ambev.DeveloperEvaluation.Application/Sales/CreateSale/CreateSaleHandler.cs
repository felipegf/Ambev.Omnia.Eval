using Ambev.DeveloperEvaluation.Domain.Entities;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">Repository for handling sales operations.</param>
    /// <param name="discountService">Service for calculating item discounts.</param>
    /// <param name="unitOfWork">Unit of Work for transaction management.</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IDiscountService discountService, IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the sale creation process.
    /// </summary>
    /// <param name="request">Command containing sale details.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The result of the sale creation process.</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        // Validate the request before processing
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Create a new sale entity
            var sale = new Sale(
                saleNumber: GenerateSaleNumber(),
                saleDate: request.SaleDate,
                customerId: request.CustomerId,
                branchId: request.BranchId,
                items: request.Items.Select(item => new SaleItem(
                    saleId: Guid.NewGuid(), // SaleItem constructor requires SaleId
                    productId: item.ProductId,
                    unitPrice: item.UnitPrice,
                    quantity: item.Quantity,
                    discount: _discountService.CalculateDiscount(item.Quantity, item.UnitPrice)
                )).ToList()
            );

            // Persist the sale
            await _saleRepository.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync();
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

    /// <summary>
    /// Generates a unique sale number.
    /// </summary>
    /// <returns>A unique string representing the sale number.</returns>
    private static string GenerateSaleNumber()
    {
        return $"SALE-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
