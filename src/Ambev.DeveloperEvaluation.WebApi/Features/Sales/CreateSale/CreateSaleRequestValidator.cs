using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for the CreateSaleRequest.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleRequestValidator"/> class.
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Customer ID cannot be empty.");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Branch ID cannot be empty.");

        RuleFor(x => x.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required.");
    }
}
