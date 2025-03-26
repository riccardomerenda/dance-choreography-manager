using DanceChoreographyManager.Shared.Models.Base;

namespace DanceChoreographyManager.Services.Course.Models;

/// <summary>
/// Represents a session of a course
/// </summary>
public class CourseSession : EntityBase
{
    /// <summary>
    /// Reference to the course
    /// </summary>
    public Guid CourseId { get; set; }
    
    /// <summary>
    /// The course
    /// </summary>
    public Course? Course { get; set; }
    
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
    /// Attendances for this session
    /// </summary>
    public List<SessionAttendance> Attendances { get; set; } = new();
    
    /// <summary>
    /// Duration of the session in minutes
    /// </summary>
    public int DurationMinutes => (int)(EndDateTime - StartDateTime).TotalMinutes;
    
    /// <summary>
    /// Whether the session has started
    /// </summary>
    public bool HasStarted => StartDateTime < DateTime.UtcNow;
    
    /// <summary>
    /// Whether the session has ended
    /// </summary>
    public bool HasEnded => EndDateTime < DateTime.UtcNow;
}