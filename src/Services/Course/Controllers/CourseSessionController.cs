using DanceChoreographyManager.Services.Course.DTOs;
using DanceChoreographyManager.Services.Course.Models;
using DanceChoreographyManager.Services.Course.Repositories;
using DanceChoreographyManager.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceChoreographyManager.Services.Course.Controllers;

[ApiController]
[Route("api/course/{courseId}/sessions")]
[Authorize]
public class CourseSessionController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ILogger<CourseSessionController> _logger;

    public CourseSessionController(
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        ILogger<CourseSessionController> logger)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all sessions for a course
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SessionDto>>>> GetSessions(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<List<SessionDto>>.CreateFailure($"Course with ID {courseId} not found"));
        }

        var sessions = await _courseRepository.GetSessionsForCourseAsync(courseId);
        var sessionDtos = sessions.Select(MapToSessionDto).ToList();

        return Ok(ApiResponse<List<SessionDto>>.CreateSuccess(sessionDtos));
    }

    /// <summary>
    /// Get a specific session
    /// </summary>
    [HttpGet("{sessionId}")]
    public async Task<ActionResult<ApiResponse<SessionDto>>> GetSession(Guid courseId, Guid sessionId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<SessionDto>.CreateFailure($"Course with ID {courseId} not found"));
        }

        var session = await _courseRepository.GetSessionByIdAsync(sessionId);
        if (session == null || session.CourseId != courseId)
        {
            return NotFound(ApiResponse<SessionDto>.CreateFailure($"Session with ID {sessionId} not found for this course"));
        }

        // Get attendances for this session
        var attendances = await _enrollmentRepository.GetAttendancesForSessionAsync(sessionId);
        var sessionDto = MapToSessionDto(session);
        
        // Only include attendances if requested with query parameter
        if (Request.Query.ContainsKey("includeAttendances") && bool.TryParse(Request.Query["includeAttendances"], out bool includeAttendances) && includeAttendances)
        {
            sessionDto.Attendances = attendances.Select(a => new AttendanceDto
            {
                Id = a.Id,
                SessionId = a.SessionId,
                DancerId = a.DancerId,
                DancerName = a.DancerName,
                Status = a.Status,
                RecordedAt = a.RecordedAt,
                Notes = a.Notes
            }).ToList();
        }

        return Ok(ApiResponse<SessionDto>.CreateSuccess(sessionDto));
    }

    /// <summary>
    /// Add a new session to a course
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<SessionDto>>> AddSession(Guid courseId, CreateSessionDto createSessionDto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<SessionDto>.CreateFailure($"Course with ID {courseId} not found"));
        }

        // Validate session times
        if (createSessionDto.EndDateTime <= createSessionDto.StartDateTime)
        {
            return BadRequest(ApiResponse<SessionDto>.CreateFailure("End time must be after start time"));
        }

        // Validate session falls within course dates
        if (createSessionDto.StartDateTime < course.StartDate || createSessionDto.EndDateTime > course.EndDate)
        {
            return BadRequest(ApiResponse<SessionDto>.CreateFailure("Session must fall within course start and end dates"));
        }

        var session = new CourseSession
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            StartDateTime = createSessionDto.StartDateTime,
            EndDateTime = createSessionDto.EndDateTime,
            Location = createSessionDto.Location ?? course.Location,
            Notes = createSessionDto.Notes,
            IsCanceled = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "system"
        };

        await _courseRepository.AddSessionAsync(session);

        _logger.LogInformation("Session added to course {CourseId}: {SessionId}", courseId, session.Id);

        var sessionDto = MapToSessionDto(session);
        return CreatedAtAction(nameof(GetSession), new { courseId = courseId, sessionId = session.Id }, 
            ApiResponse<SessionDto>.CreateSuccess(sessionDto, "Session added successfully"));
    }

    /// <summary>
    /// Update a session
    /// </summary>
    [HttpPut("{sessionId}")]
    public async Task<ActionResult<ApiResponse<SessionDto>>> UpdateSession(Guid courseId, Guid sessionId, UpdateSessionDto updateSessionDto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<SessionDto>.CreateFailure($"Course with ID {courseId} not found"));
        }

        var session = await _courseRepository.GetSessionByIdAsync(sessionId);
        if (session == null || session.CourseId != courseId)
        {
            return NotFound(ApiResponse<SessionDto>.CreateFailure($"Session with ID {sessionId} not found for this course"));
        }

        // Validate session times if provided
        if (updateSessionDto.StartDateTime.HasValue && updateSessionDto.EndDateTime.HasValue)
        {
            if (updateSessionDto.EndDateTime.Value <= updateSessionDto.StartDateTime.Value)
            {
                return BadRequest(ApiResponse<SessionDto>.CreateFailure("End time must be after start time"));
            }

            // Validate session falls within course dates
            if (updateSessionDto.StartDateTime.Value < course.StartDate || updateSessionDto.EndDateTime.Value > course.EndDate)
            {
                return BadRequest(ApiResponse<SessionDto>.CreateFailure("Session must fall within course start and end dates"));
            }
        }
        else if (updateSessionDto.StartDateTime.HasValue)
        {
            if (session.EndDateTime <= updateSessionDto.StartDateTime.Value)
            {
                return BadRequest(ApiResponse<SessionDto>.CreateFailure("End time must be after start time"));
            }

            // Validate start time falls within course dates
            if (updateSessionDto.StartDateTime.Value < course.StartDate)
            {
                return BadRequest(ApiResponse<SessionDto>.CreateFailure("Session must start after course start date"));
            }
        }
        else if (updateSessionDto.EndDateTime.HasValue)
        {
            if (updateSessionDto.EndDateTime.Value <= session.StartDateTime)
            {
                return BadRequest(ApiResponse<SessionDto>.CreateFailure("End time must be after start time"));
            }

            // Validate end time falls within course dates
            if (updateSessionDto.EndDateTime.Value > course.EndDate)
            {
                return BadRequest(ApiResponse<SessionDto>.CreateFailure("Session must end before course end date"));
            }
        }

        // Update properties if provided
        if (updateSessionDto.StartDateTime.HasValue)
            session.StartDateTime = updateSessionDto.StartDateTime.Value;
        
        if (updateSessionDto.EndDateTime.HasValue)
            session.EndDateTime = updateSessionDto.EndDateTime.Value;
        
        if (updateSessionDto.Location != null)
            session.Location = updateSessionDto.Location;
        
        if (updateSessionDto.Notes != null)
            session.Notes = updateSessionDto.Notes;
        
        if (updateSessionDto.IsCanceled.HasValue)
        {
            session.IsCanceled = updateSessionDto.IsCanceled.Value;
            
            // Set cancellation reason if session is being canceled
            if (updateSessionDto.IsCanceled.Value && updateSessionDto.CancellationReason != null)
                session.CancellationReason = updateSessionDto.CancellationReason;
            else if (!updateSessionDto.IsCanceled.Value)
                session.CancellationReason = null;
        }

        session.LastModifiedAt = DateTime.UtcNow;
        session.LastModifiedBy = User.Identity?.Name ?? "system";

        await _courseRepository.UpdateSessionAsync(session);

        _logger.LogInformation("Session updated: {SessionId} for course {CourseId}", sessionId, courseId);

        var sessionDto = MapToSessionDto(session);
        return Ok(ApiResponse<SessionDto>.CreateSuccess(sessionDto, "Session updated successfully"));
    }

    /// <summary>
    /// Delete a session
    /// </summary>
    [HttpDelete("{sessionId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSession(Guid courseId, Guid sessionId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Course with ID {courseId} not found"));
        }

        var session = await _courseRepository.GetSessionByIdAsync(sessionId);
        if (session == null || session.CourseId != courseId)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Session with ID {sessionId} not found for this course"));
        }

        await _courseRepository.DeleteSessionAsync(session);

        _logger.LogInformation("Session deleted: {SessionId} from course {CourseId}", sessionId, courseId);

        return Ok(ApiResponse<bool>.CreateSuccess(true, "Session deleted successfully"));
    }

    /// <summary>
    /// Map a CourseSession entity to a SessionDto
    /// </summary>
    private static SessionDto MapToSessionDto(CourseSession session)
    {
        return new SessionDto
        {
            Id = session.Id,
            CourseId = session.CourseId,
            StartDateTime = session.StartDateTime,
            EndDateTime = session.EndDateTime,
            Location = session.Location,
            Notes = session.Notes,
            IsCanceled = session.IsCanceled,
            CancellationReason = session.CancellationReason,
            DurationMinutes = session.DurationMinutes,
            HasStarted = session.HasStarted,
            HasEnded = session.HasEnded
        };
    }
}