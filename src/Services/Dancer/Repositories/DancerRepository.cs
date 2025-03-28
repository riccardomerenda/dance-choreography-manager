using DanceChoreographyManager.Services.Dancer.Data;
using DanceChoreographyManager.Services.Dancer.DTOs;
using DanceChoreographyManager.Shared.Utilities.Data;
using Microsoft.EntityFrameworkCore;

namespace DanceChoreographyManager.Services.Dancer.Repositories;

public class DancerRepository : Repository<Models.Dancer, DancerDbContext>, IDancerRepository
{
    public DancerRepository(DancerDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get a dancer by id with all related data
    /// </summary>
    public async Task<Models.Dancer?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(d => d.DanceStyles)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    /// <summary>
    /// Get dancers with filtering and pagination
    /// </summary>
    public async Task<(IEnumerable<Models.Dancer> Items, int TotalCount)> GetFilteredAsync(DancerFilterParams filterParams)
    {
        var query = _dbSet.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filterParams.SearchTerm))
        {
            var searchTerm = filterParams.SearchTerm.ToLower();
            query = query.Where(d => 
                d.FirstName.ToLower().Contains(searchTerm) || 
                d.LastName.ToLower().Contains(searchTerm) ||
                (d.FirstName + " " + d.LastName).ToLower().Contains(searchTerm) ||
                (d.Email != null && d.Email.ToLower().Contains(searchTerm)));
        }

        if (filterParams.Gender.HasValue)
        {
            query = query.Where(d => d.Gender == filterParams.Gender.Value);
        }

        if (filterParams.MinExperienceLevel.HasValue)
        {
            query = query.Where(d => d.ExperienceLevel >= filterParams.MinExperienceLevel.Value);
        }

        if (filterParams.IsActive.HasValue)
        {
            query = query.Where(d => d.IsActive == filterParams.IsActive.Value);
        }

        if (filterParams.MinAge.HasValue)
        {
            var maxDateOfBirth = DateTime.UtcNow.AddYears(-filterParams.MinAge.Value);
            query = query.Where(d => d.DateOfBirth <= maxDateOfBirth);
        }

        if (filterParams.MaxAge.HasValue)
        {
            var minDateOfBirth = DateTime.UtcNow.AddYears(-filterParams.MaxAge.Value - 1).AddDays(1);
            query = query.Where(d => d.DateOfBirth >= minDateOfBirth);
        }

        // Handle dance style filtering
        if (filterParams.DanceStyle.HasValue)
        {
            query = query.Where(d => d.DanceStyles.Any(ds => ds.Style == filterParams.DanceStyle.Value));
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .OrderBy(d => d.LastName)
            .ThenBy(d => d.FirstName)
            .Skip((filterParams.Page - 1) * filterParams.PageSize)
            .Take(filterParams.PageSize)
            .Include(d => d.DanceStyles)
            .ToListAsync();

        return (items, totalCount);
    }

    /// <summary>
    /// Check if a dancer with the same email already exists
    /// </summary>
    public async Task<bool> EmailExistsAsync(string email, Guid? excludeDancerId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var query = _dbSet.Where(d => d.Email == email);
        
        if (excludeDancerId.HasValue)
        {
            query = query.Where(d => d.Id != excludeDancerId.Value);
        }

        return await query.AnyAsync();
    }
}

public interface IDancerRepository : IRepository<Models.Dancer>
{
    Task<Models.Dancer?> GetByIdWithDetailsAsync(Guid id);
    Task<(IEnumerable<Models.Dancer> Items, int TotalCount)> GetFilteredAsync(DancerFilterParams filterParams);
    Task<bool> EmailExistsAsync(string email, Guid? excludeDancerId = null);
}