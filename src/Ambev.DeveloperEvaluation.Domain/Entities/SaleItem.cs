using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents an item in a sale transaction.
    /// </summary>
    public class SaleItem : BaseEntity
    {
        /// <summary>
        /// Gets the product ID.
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Gets the quantity of the product sold.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the discount applied to this item.
        /// </summary>
        public decimal Discount { get; private set; }

        /// <summary>
        /// Gets the total amount for this item.
        /// </summary>
        public decimal TotalAmount => (UnitPrice * Quantity) - Discount;

        /// <summary>
        /// Initializes a new instance of the SaleItem class.
        /// </summary>
        public SaleItem(Guid productId, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        /// <summary>
        /// Validates the SaleItem entity using the SaleItemValidator rules.
        /// </summary>
        /// <returns>A <see cref="ValidationResultDetail"/> containing validation results.</returns>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleItemValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail(result);
        }

        /// <summary>
        /// Applies a discount to this item.
        /// </summary>
        public void ApplyDiscount(decimal discount)
        {
            Discount = discount;
        }
    }
}
