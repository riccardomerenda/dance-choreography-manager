using System.ComponentModel.DataAnnotations;
using DanceChoreographyManager.Services.Course.Models;

namespace DanceChoreographyManager.Services.Course.DTOs;

/// <summary>
/// DTO for creating a new course
/// </summary>
public class CreateCourseDto
{
    /// <summary>
    /// Name of the course
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the course
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Dance style of the course
    /// </summary>
    [Required]
    public DanceStyle DanceStyle { get; set; }
    
    /// <summary>
    /// Difficulty level of the course
    /// </summary>
    [Required]
    public DifficultyLevel DifficultyLevel { get; set; }
    
    /// <summary>
    /// Start date and time of the course
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// End date and time of the course
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// Duration of each session in minutes
    /// </summary>
    [Required]
    [Range(10, 240)]
    public int DurationMinutes { get; set; }
    
    /// <summary>
    /// Capacity of the course
    /// </summary>
    [Required]
    [Range(1, 100)]
    public int Capacity { get; set; }
    
    /// <summary>
    /// Location of the course
    /// </summary>
    [StringLength(200)]
    public string? Location { get; set; }
    
    /// <summary>
    /// Instructor ID
    /// </summary>
    public Guid? InstructorId { get; set; }
    
    /// <summary>
    /// Instructor name
    /// </summary>
    [StringLength(200)]
    public string? InstructorName { get; set; }
    
    /// <summary>
    /// Price of the course
    /// </summary>
    [Required]
    [Range(0, 10000)]
    public decimal Price { get; set; }
    
    /// <summary>
    /// Currency of the price
    /// </summary>
    [StringLength(3)]
    public string Currency { get; set; } = "USD";
    
    /// <summary>
    /// Session schedule for the course
    /// </summary>
    public List<CreateSessionDto>? Sessions { get; set; }
}

/// <summary>
/// DTO for updating an existing course
/// </summary>
public class UpdateCourseDto
{
    /// <summary>
    /// Name of the course
    /// </summary>
    [StringLength(100)]
    public string? Name { get; set; }
    
    /// <summary>
    /// Description of the course
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Dance style of the course
    /// </summary>
    public DanceStyle? DanceStyle { get; set; }
    
    /// <summary>
    /// Difficulty level of the course
    /// </summary>
    public DifficultyLevel? DifficultyLevel { get; set; }
    
    /// <summary>
    /// Start date and time of the course
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// End date and time of the course
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Duration of each session in minutes
    /// </summary>
    [Range(10, 240)]
    public int? DurationMinutes { get; set; }
    
    /// <summary>
    /// Capacity of the course
    /// </summary>
    [Range(1, 100)]
    public int? Capacity { get; set; }
    
    /// <summary>
    /// Location of the course
    /// </summary>
    [StringLength(200)]
    public string? Location { get; set; }
    
    /// <summary>
    /// Instructor ID
    /// </summary>
    public Guid? InstructorId { get; set; }
    
    /// <summary>
    /// Instructor name
    /// </summary>
    [StringLength(200)]
    public string? InstructorName { get; set; }
    
    /// <summary>
    /// Whether the course is active
    /// </summary>
    public bool? IsActive { get; set; }
    
    /// <summary>
    /// Price of the course
    /// </summary>
    [Range(0, 10000)]
    public decimal? Price { get; set; }
    
    /// <summary>
    /// Currency of the price
    /// </summary>
    [StringLength(3)]
    public string? Currency { get; set; }
}

/// <summary>
/// DTO for course response
/// </summary>
public class CourseDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Name of the course
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the course
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Dance style of the course
    /// </summary>
    public DanceStyle DanceStyle { get; set; }
    
    /// <summary>
    /// Difficulty level of the course
    /// </summary>
    public DifficultyLevel DifficultyLevel { get; set; }
    
    /// <summary>
    /// Start date and time of the course
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// End date and time of the course
    /// </summary>
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// Duration of each session in minutes
    /// </summary>
    public int DurationMinutes { get; set; }
    
    /// <summary>
    /// Capacity of the course
    /// </summary>
    public int Capacity { get; set; }
    
    /// <summary>
    /// Current enrollment count
    /// </summary>
    public int EnrollmentCount { get; set; }
    
    /// <summary>
    /// Location of the course
    /// </summary>
    public string? Location { get; set; }
    
    /// <summary>
    /// Instructor ID
    /// </summary>
    public Guid? InstructorId { get; set; }
    
    /// <summary>
    /// Instructor name
    /// </summary>
    public string? InstructorName { get; set; }
    
    /// <summary>
    /// Whether the course is active
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Price of the course
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Currency of the price
    /// </summary>
    public string Currency { get; set; } = "USD";
    
    /// <summary>
    /// Whether the course is full
    /// </summary>
    public bool IsFull { get; set; }
    
    /// <summary>
    /// Whether the course has started
    /// </summary>
    public bool HasStarted { get; set; }
    
    /// <summary>
    /// Whether the course has ended
    /// </summary>
    public bool HasEnded { get; set; }
    
    /// <summary>
    /// When the course was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When the course was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
    
    /// <summary>
    /// Course sessions
    /// </summary>
    public List<SessionDto> Sessions { get; set; } = new();
}

/// <summary>
/// DTO for creating a new course session
/// </summary>
public class CreateSessionDto
{
    /// <summary>
    /// Start date and time of the session
    /// </summary>
    [Required]
    public DateTime StartDateTime { get; set; }
    
    /// <summary>
    /// End date and time of the session
    /// </summary>
    [Required]
    public DateTime EndDateTime { get; set; }
    
    /// <summary>
    /// Location of the session
    /// </summary>
    [StringLength(200)]
    public string? Location { get; set; }
    
    /// <summary>
    /// Notes for the session
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for updating an existing course session
/// </summary>
public class UpdateSessionDto
{
    /// <summary>
    /// Start date and time of the session
    /// </summary>
    public DateTime? StartDateTime { get; set; }
    
    /// <summary>
    /// End date and time of the session
    /// </summary>
    public DateTime? EndDateTime { get; set; }
    
    /// <summary>
    /// Location of the session
    /// </summary>
    [StringLength(200)]
    public string? Location { get; set; }
    
    /// <summary>
    /// Notes for the session
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Whether the session is canceled
    /// </summary>
    public bool? IsCanceled { get; set; }
    
    /// <summary>
    /// Reason for cancellation
    /// </summary>
    [StringLength(500)]
    public string? CancellationReason { get; set; }
}

/// <summary>
/// DTO for session response
/// </summary>
public class SessionDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Reference to the course
    /// </summary>
    public Guid CourseId { get; set; }
    
    /// <summary>
    /// Start date and time of the session
    /// </summary>
    public DateTime StartDateTime { get; set; }
    
    /// <summary>
    /// End date and time of the session
    /// </summary>
    public DateTime EndDateTime { get; set; }
    
    /// <summary>
    /// Location of the session
    /// </summary>
    public string? Location { get; set; }
    
    /// <summary>
    /// Notes for the session
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Whether the session is canceled
    /// </summary>
    public bool IsCanceled { get; set; }
    
    /// <summary>
    /// Reason for cancellation
    /// </summary>
    public string? CancellationReason { get; set; }
    
    /// <summary>
    /// Duration of the session in minutes
    /// </summary>
    public int DurationMinutes { get; set; }
    
    /// <summary>
    /// Whether the session has started
    /// </summary>
    public bool HasStarted { get; set; }
    
    /// <summary>
    /// Whether the session has ended
    /// </summary>
    public bool HasEnded { get; set; }
    
    /// <summary>
    /// Attendances for this session
    /// </summary>
    public List<AttendanceDto>? Attendances { get; set; }
}

/// <summary>
/// DTO for attendance response
/// </summary>
public class AttendanceDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Reference to the course session
    /// </summary>
    public Guid SessionId { get; set; }
    
    /// <summary>
    /// ID of the dancer
    /// </summary>
    public Guid DancerId { get; set; }
    
    /// <summary>
    /// Name of the dancer
    /// </summary>
    public string DancerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Attendance status
    /// </summary>
    public AttendanceStatus Status { get; set; }
    
    /// <summary>
    /// Time when the attendance was recorded
    /// </summary>
    public DateTime? RecordedAt { get; set; }
    
    /// <summary>
    /// Notes about the attendance
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Parameters for filtering courses
/// </summary>
public class CourseFilterParams
{
    /// <summary>
    /// Search term to filter by name
    /// </summary>
    public string? SearchTerm { get; set; }
    
    /// <summary>
    /// Filter by dance style
    /// </summary>
    public DanceStyle? DanceStyle { get; set; }
    
    /// <summary>
    /// Filter by difficulty level
    /// </summary>
    public DifficultyLevel? DifficultyLevel { get; set; }
    
    /// <summary>
    /// Include only active courses
    /// </summary>
    public bool? IsActive { get; set; }
    
    /// <summary>
    /// Include only courses starting after this date
    /// </summary>
    public DateTime? StartDateFrom { get; set; }
    
    /// <summary>
    /// Include only courses starting before this date
    /// </summary>
    public DateTime? StartDateTo { get; set; }
    
    /// <summary>
    /// Filter by instructor ID
    /// </summary>
    public Guid? InstructorId { get; set; }
    
    /// <summary>
    /// Include courses with available spots
    /// </summary>
    public bool? HasAvailableSpots { get; set; }
    
    /// <summary>
    /// Include only future courses (not ended)
    /// </summary>
    public bool? FutureCoursesOnly { get; set; }
    
    /// <summary>
    /// Page number (1-based)
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Page size
    /// </summary>
    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
}