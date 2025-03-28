using DanceChoreographyManager.Services.Course.Data;
using DanceChoreographyManager.Services.Course.DTOs;
using DanceChoreographyManager.Services.Course.Models;
using DanceChoreographyManager.Shared.Utilities.Data;
using Microsoft.EntityFrameworkCore;

namespace DanceChoreographyManager.Services.Course.Repositories;

public class CourseRepository : Repository<Models.Course, CourseDbContext>, ICourseRepository
{
    public CourseRepository(CourseDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get a course by id with all related data
    /// </summary>
    public async Task<Models.Course?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.Sessions)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Get courses with filtering and pagination
    /// </summary>
    public async Task<(IEnumerable<Models.Course> Items, int TotalCount)> GetFilteredAsync(CourseFilterParams filterParams)
    {
        var query = _dbSet.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filterParams.SearchTerm))
        {
            var searchTerm = filterParams.SearchTerm.ToLower();
            query = query.Where(c => 
                c.Name.ToLower().Contains(searchTerm) ||
                (c.Description != null && c.Description.ToLower().Contains(searchTerm)) ||
                (c.InstructorName != null && c.InstructorName.ToLower().Contains(searchTerm)));
        }

        if (filterParams.DanceStyle.HasValue)
        {
            query = query.Where(c => c.DanceStyle == filterParams.DanceStyle.Value);
        }

        if (filterParams.DifficultyLevel.HasValue)
        {
            query = query.Where(c => c.DifficultyLevel == filterParams.DifficultyLevel.Value);
        }

        if (filterParams.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == filterParams.IsActive.Value);
        }

        if (filterParams.StartDateFrom.HasValue)
        {
            query = query.Where(c => c.StartDate >= filterParams.StartDateFrom.Value);
        }

        if (filterParams.StartDateTo.HasValue)
        {
            query = query.Where(c => c.StartDate <= filterParams.StartDateTo.Value);
        }

        if (filterParams.InstructorId.HasValue)
        {
            query = query.Where(c => c.InstructorId == filterParams.InstructorId.Value);
        }

        if (filterParams.HasAvailableSpots.HasValue && filterParams.HasAvailableSpots.Value)
        {
            query = query.Where(c => c.EnrollmentCount < c.Capacity);
        }

        if (filterParams.FutureCoursesOnly.HasValue && filterParams.FutureCoursesOnly.Value)
        {
            var now = DateTime.UtcNow;
            query = query.Where(c => c.EndDate > now);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .OrderBy(c => c.StartDate)
            .Skip((filterParams.Page - 1) * filterParams.PageSize)
            .Take(filterParams.PageSize)
            .Include(c => c.Sessions)
            .ToListAsync();

        return (items, totalCount);
    }

    /// <summary>
    /// Check if a course with the same name already exists
    /// </summary>
    public async Task<bool> NameExistsAsync(string name, Guid? excludeCourseId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var query = _dbSet.Where(c => c.Name == name);
        
        if (excludeCourseId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCourseId.Value);
        }

        return await query.AnyAsync();
    }
    
    /// <summary>
    /// Get a course session by ID
    /// </summary>
    public async Task<CourseSession?> GetSessionByIdAsync(Guid sessionId)
    {
        return await _context.CourseSessions
            .Include(s => s.Attendances)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }
    
    /// <summary>
    /// Get all sessions for a course
    /// </summary>
    public async Task<List<CourseSession>> GetSessionsForCourseAsync(Guid courseId)
    {
        return await _context.CourseSessions
            .Where(s => s.CourseId == courseId)
            .OrderBy(s => s.StartDateTime)
            .ToListAsync();
    }
    
    /// <summary>
    /// Add a session to a course
    /// </summary>
    public async Task<CourseSession> AddSessionAsync(CourseSession session)
    {
        _context.CourseSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }
    
    /// <summary>
    /// Update a session
    /// </summary>
    public async Task<CourseSession> UpdateSessionAsync(CourseSession session)
    {
        _context.Entry(session).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return session;
    }
    
    /// <summary>
    /// Delete a session
    /// </summary>
    public async Task DeleteSessionAsync(CourseSession session)
    {
        _context.CourseSessions.Remove(session);
        await _context.SaveChangesAsync();
    }
}

public interface ICourseRepository : IRepository<Models.Course>
{
    Task<Models.Course?> GetByIdWithDetailsAsync(Guid id);
    Task<(IEnumerable<Models.Course> Items, int TotalCount)> GetFilteredAsync(CourseFilterParams filterParams);
    Task<bool> NameExistsAsync(string name, Guid? excludeCourseId = null);
    Task<CourseSession?> GetSessionByIdAsync(Guid sessionId);
    Task<List<CourseSession>> GetSessionsForCourseAsync(Guid courseId);
    Task<CourseSession> AddSessionAsync(CourseSession session);
    Task<CourseSession> UpdateSessionAsync(CourseSession session);
    Task DeleteSessionAsync(CourseSession session);
}