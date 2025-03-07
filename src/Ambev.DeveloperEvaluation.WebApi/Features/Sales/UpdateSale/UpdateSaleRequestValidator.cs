using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for the UpdateSaleRequest.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleRequestValidator"/> class.
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Sale ID cannot be empty.");

        RuleFor(x => x.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required.");
    }
}
