using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Interface for Sale repository operations.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Adds a new sale to the database.
    /// </summary>
    /// <param name="sale">The sale entity to be added.</param>
    Task AddAsync(Sale sale);

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The sale ID.</param>
    /// <returns>The sale entity, if found.</returns>
    Task<Sale?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all active (non-deleted) sales from the database.
    /// </summary>
    /// <returns>A list of sales.</returns>
    Task<IEnumerable<Sale>> GetAllAsync();

    /// <summary>
    /// Updates an existing sale in the database.
    /// </summary>
    /// <param name="sale">The sale entity to be updated.</param>
    void Update(Sale sale);

    /// <summary>
    /// Soft deletes a sale.
    /// </summary>
    /// <param name="sale">The sale entity to be deleted.</param>
    void Delete(Sale sale);
}
