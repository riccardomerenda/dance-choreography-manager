using DanceChoreographyManager.Shared.Models.Base;

namespace DanceChoreographyManager.Services.Course.Models;

/// <summary>
/// Represents a dancer's attendance at a session
/// </summary>
public class SessionAttendance : EntityBase
{
    /// <summary>
    /// Reference to the course session
    /// </summary>
    public Guid SessionId { get; set; }
    
    /// <summary>
    /// The course session
    /// </summary>
    public CourseSession? Session { get; set; }
    
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
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Unknown;
    
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
/// Status of attendance at a session
/// </summary>
public enum AttendanceStatus
{
    /// <summary>
    /// Unknown status
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Present
    /// </summary>
    Present = 1,
    
    /// <summary>
    /// Absent
    /// </summary>
    Absent = 2,
    
    /// <summary>
    /// Late
    /// </summary>
    Late = 3,
    
    /// <summary>
    /// Excused
    /// </summary>
    Excused = 4
}