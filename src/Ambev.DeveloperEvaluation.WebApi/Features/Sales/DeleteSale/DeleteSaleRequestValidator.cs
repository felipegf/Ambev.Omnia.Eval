using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// Validator for the DeleteSaleRequest.
/// </summary>
public class DeleteSaleRequestValidator : AbstractValidator<DeleteSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleRequestValidator"/> class.
    /// </summary>
    public DeleteSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sale ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Sale ID cannot be empty.");
    }

    /// <summary>
    /// Asynchronously validates the request.
    /// </summary>
    public Task<FluentValidation.Results.ValidationResult> ValidateAsync(DeleteSaleRequest request, CancellationToken cancellationToken)
    {
        return base.ValidateAsync(request, cancellationToken);
    }
}
