using DanceChoreographyManager.Services.Course.Data;
using DanceChoreographyManager.Services.Course.DTOs;
using DanceChoreographyManager.Services.Course.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceChoreographyManager.Services.Course.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly CourseDbContext _context;

    public EnrollmentRepository(CourseDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get an enrollment by ID
    /// </summary>
    public async Task<CourseEnrollment?> GetByIdAsync(Guid id)
    {
        return await _context.CourseEnrollments
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <summary>
    /// Get all enrollments for a course
    /// </summary>
    public async Task<List<CourseEnrollment>> GetEnrollmentsForCourseAsync(Guid courseId)
    {
        return await _context.CourseEnrollments
            .Where(e => e.CourseId == courseId)
            .OrderBy(e => e.DancerName)
            .ToListAsync();
    }

    /// <summary>
    /// Get all enrollments for a dancer
    /// </summary>
    public async Task<List<CourseEnrollment>> GetEnrollmentsForDancerAsync(Guid dancerId)
    {
        return await _context.CourseEnrollments
            .Include(e => e.Course)
            .Where(e => e.DancerId == dancerId)
            .OrderByDescending(e => e.Course!.StartDate)
            .ToListAsync();
    }

    /// <summary>
    /// Get enrollments with filtering and pagination
    /// </summary>
    public async Task<(IEnumerable<CourseEnrollment> Items, int TotalCount)> GetFilteredAsync(EnrollmentFilterParams filterParams)
    {
        var query = _context.CourseEnrollments
            .Include(e => e.Course)
            .AsQueryable();

        // Apply filters
        if (filterParams.CourseId.HasValue)
        {
            query = query.Where(e => e.CourseId == filterParams.CourseId.Value);
        }

        if (filterParams.DancerId.HasValue)
        {
            query = query.Where(e => e.DancerId == filterParams.DancerId.Value);
        }

        if (filterParams.Status.HasValue)
        {
            query = query.Where(e => e.Status == filterParams.Status.Value);
        }

        if (filterParams.PaymentStatus.HasValue)
        {
            query = query.Where(e => e.PaymentStatus == filterParams.PaymentStatus.Value);
        }

        if (filterParams.EnrollmentDateFrom.HasValue)
        {
            query = query.Where(e => e.EnrollmentDate >= filterParams.EnrollmentDateFrom.Value);
        }

        if (filterParams.EnrollmentDateTo.HasValue)
        {
            query = query.Where(e => e.EnrollmentDate <= filterParams.EnrollmentDateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(filterParams.SearchTerm))
        {
            var searchTerm = filterParams.SearchTerm.ToLower();
            query = query.Where(e => e.DancerName.ToLower().Contains(searchTerm) ||
                                    (e.DancerEmail != null && e.DancerEmail.ToLower().Contains(searchTerm)));
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .OrderByDescending(e => e.EnrollmentDate)
            .Skip((filterParams.Page - 1) * filterParams.PageSize)
            .Take(filterParams.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    /// <summary>
    /// Check if a dancer is already enrolled in a course
    /// </summary>
    public async Task<bool> IsEnrolledAsync(Guid courseId, Guid dancerId)
    {
        return await _context.CourseEnrollments
            .AnyAsync(e => e.CourseId == courseId && e.DancerId == dancerId);
    }

    /// <summary>
    /// Add an enrollment
    /// </summary>
    public async Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment)
    {
        // Increment the enrollment count on the course
        var course = await _context.Courses.FindAsync(enrollment.CourseId);
        if (course != null)
        {
            course.EnrollmentCount += 1;
        }

        _context.CourseEnrollments.Add(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    /// <summary>
    /// Update an enrollment
    /// </summary>
    public async Task<CourseEnrollment> UpdateAsync(CourseEnrollment enrollment)
    {
        _context.Entry(enrollment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return enrollment;
    }

    /// <summary>
    /// Delete an enrollment
    /// </summary>
    public async Task DeleteAsync(CourseEnrollment enrollment)
    {
        // Decrement the enrollment count on the course
        var course = await _context.Courses.FindAsync(enrollment.CourseId);
        if (course != null && course.EnrollmentCount > 0)
        {
            course.EnrollmentCount -= 1;
        }

        _context.CourseEnrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get all attendance records for a session
    /// </summary>
    public async Task<List<SessionAttendance>> GetAttendancesForSessionAsync(Guid sessionId)
    {
        return await _context.SessionAttendances
            .Where(a => a.SessionId == sessionId)
            .OrderBy(a => a.DancerName)
            .ToListAsync();
    }

    /// <summary>
    /// Get an attendance record by ID
    /// </summary>
    public async Task<SessionAttendance?> GetAttendanceByIdAsync(Guid attendanceId)
    {
        return await _context.SessionAttendances
            .Include(a => a.Session)
            .FirstOrDefaultAsync(a => a.Id == attendanceId);
    }

    /// <summary>
    /// Get an attendance record for a specific session and dancer
    /// </summary>
    public async Task<SessionAttendance?> GetAttendanceAsync(Guid sessionId, Guid dancerId)
    {
        return await _context.SessionAttendances
            .FirstOrDefaultAsync(a => a.SessionId == sessionId && a.DancerId == dancerId);
    }

    /// <summary>
    /// Add an attendance record
    /// </summary>
    public async Task<SessionAttendance> AddAttendanceAsync(SessionAttendance attendance)
    {
        _context.SessionAttendances.Add(attendance);
        await _context.SaveChangesAsync();
        return attendance;
    }

    /// <summary>
    /// Update an attendance record
    /// </summary>
    public async Task<SessionAttendance> UpdateAttendanceAsync(SessionAttendance attendance)
    {
        _context.Entry(attendance).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return attendance;
    }

    /// <summary>
    /// Delete an attendance record
    /// </summary>
    public async Task DeleteAttendanceAsync(SessionAttendance attendance)
    {
        _context.SessionAttendances.Remove(attendance);
        await _context.SaveChangesAsync();
    }
}

public interface IEnrollmentRepository
{
    Task<CourseEnrollment?> GetByIdAsync(Guid id);
    Task<List<CourseEnrollment>> GetEnrollmentsForCourseAsync(Guid courseId);
    Task<List<CourseEnrollment>> GetEnrollmentsForDancerAsync(Guid dancerId);
    Task<(IEnumerable<CourseEnrollment> Items, int TotalCount)> GetFilteredAsync(EnrollmentFilterParams filterParams);
    Task<bool> IsEnrolledAsync(Guid courseId, Guid dancerId);
    Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment);
    Task<CourseEnrollment> UpdateAsync(CourseEnrollment enrollment);
    Task DeleteAsync(CourseEnrollment enrollment);
    Task<List<SessionAttendance>> GetAttendancesForSessionAsync(Guid sessionId);
    Task<SessionAttendance?> GetAttendanceByIdAsync(Guid attendanceId);
    Task<SessionAttendance?> GetAttendanceAsync(Guid sessionId, Guid dancerId);
    Task<SessionAttendance> AddAttendanceAsync(SessionAttendance attendance);
    Task<SessionAttendance> UpdateAttendanceAsync(SessionAttendance attendance);
    Task DeleteAttendanceAsync(SessionAttendance attendance);
}