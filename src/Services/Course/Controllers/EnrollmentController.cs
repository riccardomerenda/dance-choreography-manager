using DanceChoreographyManager.Services.Course.DTOs;
using DanceChoreographyManager.Services.Course.Models;
using DanceChoreographyManager.Services.Course.Repositories;
using DanceChoreographyManager.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceChoreographyManager.Services.Course.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ILogger<EnrollmentController> _logger;

    public EnrollmentController(
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        ILogger<EnrollmentController> logger)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all enrollments with filtering and pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResponse<EnrollmentDto>>> GetEnrollments([FromQuery] EnrollmentFilterParams filterParams)
    {
        var (enrollments, totalCount) = await _enrollmentRepository.GetFilteredAsync(filterParams);

        var enrollmentDtos = enrollments.Select(e => new EnrollmentDto
        {
            Id = e.Id,
            CourseId = e.CourseId,
            CourseName = e.Course?.Name ?? string.Empty,
            DancerId = e.DancerId,
            DancerName = e.DancerName,
            DancerEmail = e.DancerEmail,
            EnrollmentDate = e.EnrollmentDate,
            Status = e.Status,
            PaymentStatus = e.PaymentStatus,
            AmountPaid = e.AmountPaid,
            Notes = e.Notes,
            CreatedAt = e.CreatedAt,
            LastModifiedAt = e.LastModifiedAt
        }).ToList();

        var response = new PagedResponse<EnrollmentDto>
        {
            Page = filterParams.Page,
            PageSize = filterParams.PageSize,
            TotalCount = totalCount,
            Items = enrollmentDtos
        };

        return Ok(response);
    }

    /// <summary>
    /// Get an enrollment by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> GetEnrollment(Guid id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound(ApiResponse<EnrollmentDto>.CreateFailure($"Enrollment with ID {id} not found"));
        }

        var enrollmentDto = new EnrollmentDto
        {
            Id = enrollment.Id,
            CourseId = enrollment.CourseId,
            CourseName = enrollment.Course?.Name ?? string.Empty,
            DancerId = enrollment.DancerId,
            DancerName = enrollment.DancerName,
            DancerEmail = enrollment.DancerEmail,
            EnrollmentDate = enrollment.EnrollmentDate,
            Status = enrollment.Status,
            PaymentStatus = enrollment.PaymentStatus,
            AmountPaid = enrollment.AmountPaid,
            Notes = enrollment.Notes,
            CreatedAt = enrollment.CreatedAt,
            LastModifiedAt = enrollment.LastModifiedAt
        };

        return Ok(ApiResponse<EnrollmentDto>.CreateSuccess(enrollmentDto));
    }

    /// <summary>
    /// Get all enrollments for a dancer
    /// </summary>
    [HttpGet("dancer/{dancerId}")]
    public async Task<ActionResult<ApiResponse<List<EnrollmentDto>>>> GetDancerEnrollments(Guid dancerId)
    {
        var enrollments = await _enrollmentRepository.GetEnrollmentsForDancerAsync(dancerId);
        var enrollmentDtos = enrollments.Select(e => new EnrollmentDto
        {
            Id = e.Id,
            CourseId = e.CourseId,
            CourseName = e.Course?.Name ?? string.Empty,
            DancerId = e.DancerId,
            DancerName = e.DancerName,
            DancerEmail = e.DancerEmail,
            EnrollmentDate = e.EnrollmentDate,
            Status = e.Status,
            PaymentStatus = e.PaymentStatus,
            AmountPaid = e.AmountPaid,
            Notes = e.Notes,
            CreatedAt = e.CreatedAt,
            LastModifiedAt = e.LastModifiedAt
        }).ToList();

        return Ok(ApiResponse<List<EnrollmentDto>>.CreateSuccess(enrollmentDtos));
    }

    /// <summary>
    /// Get all enrollments for a course
    /// </summary>
    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<ApiResponse<List<EnrollmentDto>>>> GetCourseEnrollments(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<List<EnrollmentDto>>.CreateFailure($"Course with ID {courseId} not found"));
        }

        var enrollments = await _enrollmentRepository.GetEnrollmentsForCourseAsync(courseId);
        var enrollmentDtos = enrollments.Select(e => new EnrollmentDto
        {
            Id = e.Id,
            CourseId = e.CourseId,
            CourseName = course.Name,
            DancerId = e.DancerId,
            DancerName = e.DancerName,
            DancerEmail = e.DancerEmail,
            EnrollmentDate = e.EnrollmentDate,
            Status = e.Status,
            PaymentStatus = e.PaymentStatus,
            AmountPaid = e.AmountPaid,
            Notes = e.Notes,
            CreatedAt = e.CreatedAt,
            LastModifiedAt = e.LastModifiedAt
        }).ToList();

        return Ok(ApiResponse<List<EnrollmentDto>>.CreateSuccess(enrollmentDtos));
    }

    /// <summary>
    /// Create a new enrollment
    /// </summary>
    [HttpPost("course/{courseId}")]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> CreateEnrollment(Guid courseId, CreateEnrollmentDto createEnrollmentDto)
    {
        var course = await _courseRepository.GetByIdWithDetailsAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<EnrollmentDto>.CreateFailure($"Course with ID {courseId} not found"));
        }

        // Check if the course is full
        if (course.IsFull)
        {
            return BadRequest(ApiResponse<EnrollmentDto>.CreateFailure("This course is full"));
        }

        // Check if the dancer is already enrolled
        if (await _enrollmentRepository.IsEnrolledAsync(courseId, createEnrollmentDto.DancerId))
        {
            return BadRequest(ApiResponse<EnrollmentDto>.CreateFailure("This dancer is already enrolled in this course"));
        }

        var enrollment = new CourseEnrollment
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            DancerId = createEnrollmentDto.DancerId,
            DancerName = createEnrollmentDto.DancerName,
            DancerEmail = createEnrollmentDto.DancerEmail,
            EnrollmentDate = DateTime.UtcNow,
            Status = createEnrollmentDto.Status,
            PaymentStatus = createEnrollmentDto.PaymentStatus,
            AmountPaid = createEnrollmentDto.AmountPaid,
            Notes = createEnrollmentDto.Notes,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "system"
        };

        await _enrollmentRepository.AddAsync(enrollment);

        _logger.LogInformation("Enrollment created: {EnrollmentId} - Dancer: {DancerId} ({DancerName}) in Course: {CourseId} ({CourseName})", 
            enrollment.Id, enrollment.DancerId, enrollment.DancerName, courseId, course.Name);

        var enrollmentDto = new EnrollmentDto
        {
            Id = enrollment.Id,
            CourseId = enrollment.CourseId,
            CourseName = course.Name,
            DancerId = enrollment.DancerId,
            DancerName = enrollment.DancerName,
            DancerEmail = enrollment.DancerEmail,
            EnrollmentDate = enrollment.EnrollmentDate,
            Status = enrollment.Status,
            PaymentStatus = enrollment.PaymentStatus,
            AmountPaid = enrollment.AmountPaid,
            Notes = enrollment.Notes,
            CreatedAt = enrollment.CreatedAt,
            LastModifiedAt = enrollment.LastModifiedAt
        };

        return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, 
            ApiResponse<EnrollmentDto>.CreateSuccess(enrollmentDto, "Enrollment created successfully"));
    }

    /// <summary>
    /// Update an enrollment
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> UpdateEnrollment(Guid id, UpdateEnrollmentDto updateEnrollmentDto)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound(ApiResponse<EnrollmentDto>.CreateFailure($"Enrollment with ID {id} not found"));
        }

        // Update properties if provided
        if (updateEnrollmentDto.Status.HasValue)
            enrollment.Status = updateEnrollmentDto.Status.Value;
        
        if (updateEnrollmentDto.PaymentStatus.HasValue)
            enrollment.PaymentStatus = updateEnrollmentDto.PaymentStatus.Value;
        
        if (updateEnrollmentDto.AmountPaid.HasValue)
            enrollment.AmountPaid = updateEnrollmentDto.AmountPaid.Value;
        
        if (updateEnrollmentDto.Notes != null)
            enrollment.Notes = updateEnrollmentDto.Notes;

        enrollment.LastModifiedAt = DateTime.UtcNow;
        enrollment.LastModifiedBy = User.Identity?.Name ?? "system";

        await _enrollmentRepository.UpdateAsync(enrollment);

        _logger.LogInformation("Enrollment updated: {EnrollmentId}", enrollment.Id);

        var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
        var enrollmentDto = new EnrollmentDto
        {
            Id = enrollment.Id,
            CourseId = enrollment.CourseId,
            CourseName = course?.Name ?? string.Empty,
            DancerId = enrollment.DancerId,
            DancerName = enrollment.DancerName,
            DancerEmail = enrollment.DancerEmail,
            EnrollmentDate = enrollment.EnrollmentDate,
            Status = enrollment.Status,
            PaymentStatus = enrollment.PaymentStatus,
            AmountPaid = enrollment.AmountPaid,
            Notes = enrollment.Notes,
            CreatedAt = enrollment.CreatedAt,
            LastModifiedAt = enrollment.LastModifiedAt
        };

        return Ok(ApiResponse<EnrollmentDto>.CreateSuccess(enrollmentDto, "Enrollment updated successfully"));
    }

    /// <summary>
    /// Delete an enrollment
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteEnrollment(Guid id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Enrollment with ID {id} not found"));
        }

        await _enrollmentRepository.DeleteAsync(enrollment);

        _logger.LogInformation("Enrollment deleted: {EnrollmentId} - Dancer: {DancerId} from Course: {CourseId}", 
            enrollment.Id, enrollment.DancerId, enrollment.CourseId);

        return Ok(ApiResponse<bool>.CreateSuccess(true, "Enrollment deleted successfully"));
    }
}