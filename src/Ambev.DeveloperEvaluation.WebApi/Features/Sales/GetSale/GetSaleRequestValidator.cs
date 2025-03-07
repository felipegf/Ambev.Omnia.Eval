﻿using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Validator for the GetSaleRequest.
/// </summary>
public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleRequestValidator"/> class.
    /// </summary>
    public GetSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sale ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Sale ID cannot be empty.");
    }
}
