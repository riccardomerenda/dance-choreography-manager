using DanceChoreographyManager.Services.Course.DTOs;
using DanceChoreographyManager.Services.Course.Models;
using DanceChoreographyManager.Services.Course.Repositories;
using DanceChoreographyManager.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceChoreographyManager.Services.Course.Controllers;

[ApiController]
[Route("api/session/{sessionId}/attendance")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        ILogger<AttendanceController> logger)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all attendance records for a session
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AttendanceDto>>>> GetAttendances(Guid sessionId)
    {
        var session = await _courseRepository.GetSessionByIdAsync(sessionId);
        if (session == null)
        {
            return NotFound(ApiResponse<List<AttendanceDto>>.CreateFailure($"Session with ID {sessionId} not found"));
        }

        var attendances = await _enrollmentRepository.GetAttendancesForSessionAsync(sessionId);
        var attendanceDtos = attendances.Select(a => new AttendanceDto
        {
            Id = a.Id,
            SessionId = a.SessionId,
            DancerId = a.DancerId,
            DancerName = a.DancerName,
            Status = a.Status,
            RecordedAt = a.RecordedAt,
            Notes = a.Notes
        }).ToList();

        return Ok(ApiResponse<List<AttendanceDto>>.CreateSuccess(attendanceDtos));
    }

    /// <summary>
    /// Get attendance for a specific dancer in a session
    /// </summary>
    [HttpGet("dancer/{dancerId}")]
    public async Task<ActionResult<ApiResponse<AttendanceDto>>> GetAttendance(Guid sessionId, Guid dancerId)
    {
        var session = await _courseRepository.GetSessionByIdAsync(sessionId);
        if (session == null)
        {
            return NotFound(ApiResponse<AttendanceDto>.CreateFailure($"Session with ID {sessionId} not found"));
        }

        var attendance = await _enrollmentRepository.GetAttendanceAsync(sessionId, dancerId);
        if (attendance == null)
        {
            return NotFound(ApiResponse<AttendanceDto>.CreateFailure($"Attendance record not found for dancer {dancerId} in session {sessionId}"));
        }

        var attendanceDto = new AttendanceDto
        {
            Id = attendance.Id,
            SessionId = attendance.SessionId,
            DancerId = attendance.DancerId,
            DancerName = attendance.DancerName,
            Status = attendance.Status,
            RecordedAt = attendance.RecordedAt,
            Notes = attendance.Notes
        };

        return Ok(ApiResponse<AttendanceDto>.CreateSuccess(attendanceDto));
    }

    /// <summary>
    /// Record or update attendance for a dancer in a session
    /// </summary>
    [HttpPut("dancer/{dancerId}")]
    public async Task<ActionResult<ApiResponse<AttendanceDto>>> RecordAttendance(Guid sessionId, Guid dancerId, UpdateAttendanceDto updateAttendanceDto)
    {
        var session = await _courseRepository.GetSessionByIdAsync(sessionId);
        if (session == null)
        {
            return NotFound(ApiResponse<AttendanceDto>.CreateFailure($"Session with ID {sessionId} not found"));
        }

        // Get the course
        var course = await _courseRepository.GetByIdAsync(session.CourseId);
        if (course == null)
        {
            return NotFound(ApiResponse<AttendanceDto>.CreateFailure($"Course with ID {session.CourseId} not found"));
        }

        // Check if the dancer is enrolled in the course
        if (!await _enrollmentRepository.IsEnrolledAsync(course.Id, dancerId))
        {
            return BadRequest(ApiResponse<AttendanceDto>.CreateFailure($"Dancer with ID {dancerId} is not enrolled in this course"));
        }

        // Check if an attendance record already exists
        var attendance = await _enrollmentRepository.GetAttendanceAsync(sessionId, dancerId);
        var isNewRecord = attendance == null;

        if (isNewRecord)
        {
            // Get the enrollment to get the dancer name
            var enrollments = await _enrollmentRepository.GetEnrollmentsForCourseAsync(course.Id);
            var enrollment = enrollments.FirstOrDefault(e => e.DancerId == dancerId);
            if (enrollment == null)
            {
                return BadRequest(ApiResponse<AttendanceDto>.CreateFailure($"Enrollment record not found for dancer {dancerId} in course {course.Id}"));
            }

            // Create a new attendance record
            attendance = new SessionAttendance
            {
                Id = Guid.NewGuid(),
                SessionId = sessionId,
                DancerId = dancerId,
                DancerName = enrollment.DancerName,
                Status = updateAttendanceDto.Status,
                RecordedAt = DateTime.UtcNow,
                Notes = updateAttendanceDto.Notes,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User?.Identity?.Name ?? "system"
            };

            await _enrollmentRepository.AddAttendanceAsync(attendance);
            _logger.LogInformation("Attendance recorded: {AttendanceId} - Dancer: {DancerId} ({DancerName}) in Session: {SessionId}", 
                attendance.Id, dancerId, attendance.DancerName, sessionId);
        }
        else
        {
            if (attendance == null)
            {
                return BadRequest(ApiResponse<AttendanceDto>.CreateFailure("Unexpected error: Attendance record is null"));
            }

            // Update existing attendance record
            attendance.Status = updateAttendanceDto.Status;
            attendance.RecordedAt = DateTime.UtcNow;
            if (updateAttendanceDto.Notes != null)
                attendance.Notes = updateAttendanceDto.Notes;
            
            attendance.LastModifiedAt = DateTime.UtcNow;
            attendance.LastModifiedBy = User?.Identity?.Name ?? "system";

            await _enrollmentRepository.UpdateAttendanceAsync(attendance);
            _logger.LogInformation("Attendance updated: {AttendanceId} - Dancer: {DancerId} in Session: {SessionId}", 
                attendance.Id, dancerId, sessionId);
        }

        var attendanceDto = new AttendanceDto
        {
            Id = attendance.Id,
            SessionId = attendance.SessionId,
            DancerId = attendance.DancerId,
            DancerName = attendance.DancerName,
            Status = attendance.Status,
            RecordedAt = attendance.RecordedAt,
            Notes = attendance.Notes
        };

        var message = isNewRecord ? "Attendance recorded successfully" : "Attendance updated successfully";
        return Ok(ApiResponse<AttendanceDto>.CreateSuccess(attendanceDto, message));
    }

    /// <summary>
    /// Delete an attendance record
    /// </summary>
    [HttpDelete("dancer/{dancerId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteAttendance(Guid sessionId, Guid dancerId)
    {
        var session = await _courseRepository.GetSessionByIdAsync(sessionId);
        if (session == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Session with ID {sessionId} not found"));
        }

        var attendance = await _enrollmentRepository.GetAttendanceAsync(sessionId, dancerId);
        if (attendance == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Attendance record not found for dancer {dancerId} in session {sessionId}"));
        }

        await _enrollmentRepository.DeleteAttendanceAsync(attendance);

        _logger.LogInformation("Attendance deleted: {AttendanceId} - Dancer: {DancerId} from Session: {SessionId}", 
            attendance.Id, dancerId, sessionId);

        return Ok(ApiResponse<bool>.CreateSuccess(true, "Attendance record deleted successfully"));
    }
}