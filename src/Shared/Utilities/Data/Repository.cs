using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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

/// <summary>
/// Generic repository implementation
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
/// <typeparam name="TContext">DbContext type</typeparam>
public abstract class Repository<T, TContext> : IRepository<T>
    where T : class
    where TContext : DbContext
{
    protected readonly TContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(TContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var entry = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }

    public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await CountAsync();
        var items = await _dbSet
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>> predicate, int page, int pageSize)
    {
        var query = _dbSet.Where(predicate);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}