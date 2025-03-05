using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    /// <summary>
    /// Validator for Sale entity.
    /// </summary>
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(sale => sale.SaleNumber)
                .NotEmpty().WithMessage("Sale number cannot be empty.")
                .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

            RuleFor(sale => sale.SaleDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(sale => sale.Branch)
                .NotEmpty().WithMessage("Branch cannot be empty.");

            RuleFor(sale => sale.CustomerId)
                .NotEmpty().WithMessage("Customer ID cannot be empty.");
        }
    }
}
