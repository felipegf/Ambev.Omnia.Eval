using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for updating a sale.
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty().WithMessage("Sale ID is required.");
        RuleFor(x => x.SaleDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");
        RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemValidator());
    }
}

/// <summary>
/// Validator for updating a sale item.
/// </summary>
public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
{
    public UpdateSaleItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        RuleFor(x => x.Quantity).LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 units of the same product.");
    }
}
