using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for <see cref="CreateSaleCommand"/>.
/// Ensures that all required fields are properly populated and valid.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes validation rules for creating a sale.
    /// </summary>
    public CreateSaleCommandValidator()
    {
        // Validate CustomerId
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID must not be empty.");

        // Validate BranchId
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID must not be empty.");

        // Validate SaleDate (it cannot be in the future)
        RuleFor(x => x.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future.");

        // Validate that at least one item exists in the sale
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sale must contain at least one item.");

        // Validate each sale item
        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemCommandValidator());
    }
}

/// <summary>
/// Validator for <see cref="CreateSaleItemCommand"/>.
/// Ensures that each sale item has valid values.
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes validation rules for sale items.
    /// </summary>
    public CreateSaleItemCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID must not be empty.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
            .Must((item, discount) => discount <= item.UnitPrice * item.Quantity)
            .WithMessage("Discount cannot exceed total item price.");
    }
}
