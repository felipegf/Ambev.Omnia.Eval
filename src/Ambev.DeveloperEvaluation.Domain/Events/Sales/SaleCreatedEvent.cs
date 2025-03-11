using Ambev.DeveloperEvaluation.Common.Events;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Event triggered when a new sale is created.
/// </summary>
public sealed class SaleCreatedEvent : EventBase
{
    /// <summary>
    /// Gets the sale number.
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Gets the sale date.
    /// </summary>
    public DateTime SaleDate { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleCreatedEvent"/> class.
    /// </summary>
    /// <param name="aggregateId">The sale ID.</param>
    /// <param name="saleNumber">The sale number.</param>
    /// <param name="saleDate">The sale date.</param>
    public SaleCreatedEvent(Guid aggregateId, string saleNumber, DateTime saleDate)
    : base(aggregateId)
    {
        SaleNumber = saleNumber ?? throw new ArgumentNullException(nameof(saleNumber));
        SaleDate = saleDate;
    }

}
