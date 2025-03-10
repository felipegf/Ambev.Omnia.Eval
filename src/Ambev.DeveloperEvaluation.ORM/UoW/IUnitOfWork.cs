namespace Ambev.DeveloperEvaluation.ORM.UoW;

/// <summary>
/// Interface for Unit of Work to manage transactions.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all pending changes in a single transaction.
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the active transaction.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the active transaction.
    /// </summary>
    Task RollbackTransactionAsync();
}
