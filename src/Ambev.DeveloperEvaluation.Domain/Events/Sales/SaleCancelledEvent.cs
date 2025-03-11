using Ambev.DeveloperEvaluation.Common.Events;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Event triggered when a sale is cancelled.
/// </summary>
public sealed class SaleCancelledEvent : EventBase, IEvent
{
    /// <summary>
    /// Gets the cancellation reason.
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleCancelledEvent"/> class.
    /// </summary>
    /// <param name="aggregateId">The sale ID.</param>
    /// <param name="reason">The cancellation reason.</param>
    public SaleCancelledEvent(Guid aggregateId, string reason)
    : base(aggregateId)
    {
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
    }

}
