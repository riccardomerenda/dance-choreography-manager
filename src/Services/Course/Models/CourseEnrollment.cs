using DanceChoreographyManager.Shared.Models.Base;

namespace DanceChoreographyManager.Services.Course.Models;

/// <summary>
/// Represents a dancer's enrollment in a course
/// </summary>
public class CourseEnrollment : EntityBase
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
    /// ID of the dancer
    /// </summary>
    public Guid DancerId { get; set; }
    
    /// <summary>
    /// Name of the dancer
    /// </summary>
    public string DancerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Email of the dancer
    /// </summary>
    public string? DancerEmail { get; set; }
    
    /// <summary>
    /// Enrollment date
    /// </summary>
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Status of the enrollment
    /// </summary>
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    
    /// <summary>
    /// Payment status of the enrollment
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    
    /// <summary>
    /// Amount paid
    /// </summary>
    public decimal AmountPaid { get; set; }
    
    /// <summary>
    /// Notes about the enrollment
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Status of an enrollment
/// </summary>
public enum EnrollmentStatus
{
    /// <summary>
    /// Active enrollment
    /// </summary>
    Active = 0,
    
    /// <summary>
    /// Completed enrollment
    /// </summary>
    Completed = 1,
    
    /// <summary>
    /// Dropped enrollment
    /// </summary>
    Dropped = 2,
    
    /// <summary>
    /// Waitlisted enrollment
    /// </summary>
    Waitlisted = 3,
    
    /// <summary>
    /// Canceled enrollment
    /// </summary>
    Canceled = 4
}

/// <summary>
/// Payment status of an enrollment
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Pending payment
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// Paid
    /// </summary>
    Paid = 1,
    
    /// <summary>
    /// Partially paid
    /// </summary>
    PartiallyPaid = 2,
    
    /// <summary>
    /// Refunded
    /// </summary>
    Refunded = 3,
    
    /// <summary>
    /// Failed payment
    /// </summary>
    Failed = 4
}