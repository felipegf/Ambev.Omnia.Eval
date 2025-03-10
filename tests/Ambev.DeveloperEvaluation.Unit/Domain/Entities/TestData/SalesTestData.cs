namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides test data generation for Sale and SaleItem entities.
/// Uses the Bogus library to create valid and invalid test cases.
/// </summary>
public static class SalesTestData
{
    private static readonly Faker Faker = new Faker("pt_BR");

    /// <summary>
    /// Generates a valid SaleItem entity using the required constructor.
    /// </summary>
    private static SaleItem GenerateValidSaleItem(Guid saleId)
    {
        var productId = Guid.NewGuid();
        var unitPrice = Faker.Random.Decimal(1.00m, 500.00m);
        var quantity = Faker.Random.Int(1, 20);
        var discount = Faker.Random.Decimal(0, unitPrice * 0.2m); // Max 20% discount

        return new SaleItem(saleId, productId, unitPrice, quantity, discount);
    }

    /// <summary>
    /// Generates a valid Sale entity using the required constructor.
    /// </summary>
    public static Sale GenerateValidSale()
    {
        var saleId = Guid.NewGuid();
        var saleNumber = $"SALE-{Faker.Random.AlphaNumeric(8).ToUpper()}";
        var saleDate = Faker.Date.Past(1);
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        // Create a new sale instance using the constructor
        var sale = new Sale(saleNumber, saleDate, customerId, branchId, new List<SaleItem>());

        // Add valid sale items using the method from Sale class
        int itemCount = Faker.Random.Int(1, 5);
        for (int i = 0; i < itemCount; i++)
        {
            var saleItem = GenerateValidSaleItem(sale.Id);
            sale.AddSaleItem(saleItem);
        }

        return sale;
    }

    /// <summary>
    /// Generates an invalid Sale entity with incorrect values.
    /// </summary>
    public static Sale GenerateInvalidSale()
    {
        var saleId = Guid.NewGuid();
        var saleNumber = ""; // Invalid empty sale number
        var saleDate = Faker.Date.Future(2); // Future date (invalid)
        var customerId = Guid.Empty; // Invalid CustomerId
        var branchId = Guid.Empty; // Invalid BranchId

        var sale = new Sale(saleNumber, saleDate, customerId, branchId, new List<SaleItem>());

        // Add an invalid SaleItem
        sale.AddSaleItem(GenerateInvalidSaleItem(sale.Id));

        return sale;
    }

    /// <summary>
    /// Generates an invalid SaleItem entity.
    /// </summary>
    public static SaleItem GenerateInvalidSaleItem(Guid saleId)
    {
        var productId = Guid.Empty; // Invalid ProductId
        var unitPrice = -50; // Invalid negative price
        var quantity = 100; // Invalid excessive quantity
        var discount = 1000; // Discount higher than total price (invalid)

        return new SaleItem(saleId, productId, unitPrice, quantity, discount);
    }
}
