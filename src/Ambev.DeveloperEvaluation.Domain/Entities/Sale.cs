using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sales transaction.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the sale number.
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Gets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; }

    /// <summary>
    /// Gets the customer ID associated with the sale.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the branch where the sale was made.
    /// </summary>
    public Guid BranchId { get; }

    /// <summary>
    /// Gets the total sale amount.
    /// </summary>
    public decimal TotalAmount => SaleItems.Sum(i => i.TotalAmount);

    /// <summary>
    /// Gets the creation date of the sale.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the last update date of the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Indicates whether the sale is canceled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Indicates whether the sale is deleted (soft delete).
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Gets the list of items in the sale.
    /// </summary>
    private readonly List<SaleItem> _saleItems = new();
    public IReadOnlyCollection<SaleItem> SaleItems => _saleItems.AsReadOnly();

    /// <summary>
    /// Protected Constructor (EF Core Needed)
    /// </summary>
    protected Sale() { }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale(string saleNumber, DateTime saleDate, Guid customerId, Guid branchId, List<SaleItem> items)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;
        CreatedAt = DateTime.UtcNow;
        _saleItems = items ?? [];
        IsDeleted = false;
    }

    /// <summary>
    /// Adds a new item to the sale.
    /// </summary>
    /// <param name="item">The sale item to add.</param>
    public void AddSaleItem(SaleItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _saleItems.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    /// <param name="item">The sale item to remove.</param>
    public void RemoveSaleItem(SaleItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _saleItems.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the sale transaction.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the sale as deleted (soft delete).
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the Sale entity using the SaleValidator rules.
    /// </summary>
    /// <returns>A <see cref="ValidationResultDetail"/> containing validation results.</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail(result);
    }
}
