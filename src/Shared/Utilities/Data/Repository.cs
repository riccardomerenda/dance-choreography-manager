using System.Linq.Expressions;

namespace DanceChoreographyManager.Shared.Utilities.Data;

/// <summary>
/// Generic repository interface
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns>All entities</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get entities by predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <returns>Filtered entities</returns>
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Find entity by id
    /// </summary>
    /// <param name="id">Entity id</param>
    /// <returns>Entity or null</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Find first entity matching predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <returns>Entity or null</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Add new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <returns>Added entity</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <returns>Updated entity</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    /// <returns>Task</returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Delete entity by id
    /// </summary>
    /// <param name="id">Entity id</param>
    /// <returns>Task</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Check if any entity matches predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <returns>True if any entity matches</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Count entities
    /// </summary>
    /// <returns>Number of entities</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Count entities matching predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <returns>Number of matching entities</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get paged entities
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paged entities</returns>
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);

    /// <summary>
    /// Get paged entities matching predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paged entities</returns>
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>> predicate, int page, int pageSize);
}