using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ambev.DeveloperEvaluation.ORM.UoW;

/// <summary>
/// Implementation of Unit of Work pattern.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DefaultContext _context;
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Initializes a new instance of <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(DefaultContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction has not been started.");

        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction has not been started.");

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
