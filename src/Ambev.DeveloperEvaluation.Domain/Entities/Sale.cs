using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sales transaction.
    /// </summary>
    public class Sale : BaseEntity
    {
        /// <summary>
        /// Gets the sale number.
        /// </summary>
        public string SaleNumber { get; private set; }

        /// <summary>
        /// Gets the date when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; private set; }

        /// <summary>
        /// Gets the customer ID associated with the sale.
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Gets the total sale amount.
        /// </summary>
        public decimal TotalAmount => SaleItems.Sum(i => i.TotalAmount);

        /// <summary>
        /// Gets the branch where the sale was made.
        /// </summary>
        public string Branch { get; private set; }

        /// <summary>
        /// Gets the list of items in the sale.
        /// </summary>
        public List<SaleItem> SaleItems { get; private set; }

        /// <summary>
        /// Gets the status of the sale (Cancelled or Not).
        /// </summary>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Sale class.
        /// </summary>
        public Sale(string saleNumber, DateTime saleDate, Guid customerId, string branch, List<SaleItem> items)
        {
            SaleNumber = saleNumber;
            SaleDate = saleDate;
            CustomerId = customerId;
            Branch = branch;
            SaleItems = items ?? throw new ArgumentNullException(nameof(items));
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

        /// <summary>
        /// Cancels the sale transaction.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}