using Ambev.DeveloperEvaluation.Common.Events;

/// <summary>
/// Event triggered when a sale is updated.
/// </summary>
public sealed class SaleUpdatedEvent : EventBase, IEvent
{
    public string SaleNumber { get; }
    public DateTime SaleDate { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleUpdatedEvent"/> class.
    /// </summary>
    public SaleUpdatedEvent(Guid aggregateId, string saleNumber, DateTime saleDate)
    : base(aggregateId)
    {
        SaleNumber = saleNumber ?? throw new ArgumentNullException(nameof(saleNumber));
        SaleDate = saleDate;
    }
}
