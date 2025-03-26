using DanceChoreographyManager.Shared.Models.Base;

namespace DanceChoreographyManager.Services.Course.Models;

/// <summary>
/// Represents a dance course
/// </summary>
public class Course : EntityBase
{
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
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Price of the course
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Currency of the price
    /// </summary>
    public string Currency { get; set; } = "USD";
    
    /// <summary>
    /// Course sessions
    /// </summary>
    public List<CourseSession> Sessions { get; set; } = new();
    
    /// <summary>
    /// Course enrollments
    /// </summary>
    public List<CourseEnrollment> Enrollments { get; set; } = new();
    
    /// <summary>
    /// Whether the course is full
    /// </summary>
    public bool IsFull => EnrollmentCount >= Capacity;
    
    /// <summary>
    /// Whether the course has started
    /// </summary>
    public bool HasStarted => StartDate < DateTime.UtcNow;
    
    /// <summary>
    /// Whether the course has ended
    /// </summary>
    public bool HasEnded => EndDate < DateTime.UtcNow;
}

/// <summary>
/// Dance styles available for courses
/// </summary>
public enum DanceStyle
{
    /// <summary>
    /// Ballet
    /// </summary>
    Ballet = 0,
    
    /// <summary>
    /// Contemporary
    /// </summary>
    Contemporary = 1,
    
    /// <summary>
    /// Jazz
    /// </summary>
    Jazz = 2,
    
    /// <summary>
    /// Hip Hop
    /// </summary>
    HipHop = 3,
    
    /// <summary>
    /// Tap
    /// </summary>
    Tap = 4,
    
    /// <summary>
    /// Ballroom
    /// </summary>
    Ballroom = 5,
    
    /// <summary>
    /// Latin
    /// </summary>
    Latin = 6,
    
    /// <summary>
    /// Salsa
    /// </summary>
    Salsa = 7,
    
    /// <summary>
    /// Breakdance
    /// </summary>
    Breakdance = 8,
    
    /// <summary>
    /// Folk
    /// </summary>
    Folk = 9,
    
    /// <summary>
    /// Modern
    /// </summary>
    Modern = 10,
    
    /// <summary>
    /// Swing
    /// </summary>
    Swing = 11,
    
    /// <summary>
    /// Lyrical
    /// </summary>
    Lyrical = 12,
    
    /// <summary>
    /// Other
    /// </summary>
    Other = 99
}

/// <summary>
/// Difficulty levels for courses
/// </summary>
public enum DifficultyLevel
{
    /// <summary>
    /// Beginner level
    /// </summary>
    Beginner = 0,
    
    /// <summary>
    /// Intermediate level
    /// </summary>
    Intermediate = 1,
    
    /// <summary>
    /// Advanced level
    /// </summary>
    Advanced = 2,
    
    /// <summary>
    /// All levels
    /// </summary>
    AllLevels = 3
}