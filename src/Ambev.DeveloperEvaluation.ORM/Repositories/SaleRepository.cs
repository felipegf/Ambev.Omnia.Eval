﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for Sale entity.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
    }

    /// <inheritdoc />
    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales
            .Include(s => s.SaleItems)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.SaleItems)
            .Where(s => !s.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public void Update(Sale sale)
    {
        _context.Sales.Update(sale);
    }

    /// <inheritdoc />
    public void Delete(Sale sale)
    {
        sale.Delete();
        _context.Sales.Update(sale);
    }
}
