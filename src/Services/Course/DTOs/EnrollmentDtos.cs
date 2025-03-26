using System.ComponentModel.DataAnnotations;
using DanceChoreographyManager.Services.Course.Models;

namespace DanceChoreographyManager.Services.Course.DTOs;

/// <summary>
/// DTO for creating a new enrollment
/// </summary>
public class CreateEnrollmentDto
{
    /// <summary>
    /// ID of the dancer
    /// </summary>
    [Required]
    public Guid DancerId { get; set; }
    
    /// <summary>
    /// Name of the dancer
    /// </summary>
    [Required]
    [StringLength(200)]
    public string DancerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Email of the dancer
    /// </summary>
    [EmailAddress]
    [StringLength(255)]
    public string? DancerEmail { get; set; }
    
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
    [Range(0, 10000)]
    public decimal AmountPaid { get; set; }
    
    /// <summary>
    /// Notes about the enrollment
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for updating an existing enrollment
/// </summary>
public class UpdateEnrollmentDto
{
    /// <summary>
    /// Status of the enrollment
    /// </summary>
    public EnrollmentStatus? Status { get; set; }
    
    /// <summary>
    /// Payment status of the enrollment
    /// </summary>
    public PaymentStatus? PaymentStatus { get; set; }
    
    /// <summary>
    /// Amount paid
    /// </summary>
    [Range(0, 10000)]
    public decimal? AmountPaid { get; set; }
    
    /// <summary>
    /// Notes about the enrollment
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for enrollment response
/// </summary>
public class EnrollmentDto
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
    /// Course name
    /// </summary>
    public string CourseName { get; set; } = string.Empty;
    
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
    public DateTime EnrollmentDate { get; set; }
    
    /// <summary>
    /// Status of the enrollment
    /// </summary>
    public EnrollmentStatus Status { get; set; }
    
    /// <summary>
    /// Payment status of the enrollment
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }
    
    /// <summary>
    /// Amount paid
    /// </summary>
    public decimal AmountPaid { get; set; }
    
    /// <summary>
    /// Notes about the enrollment
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// When the enrollment was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When the enrollment was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
}

/// <summary>
/// DTO for updating attendance
/// </summary>
public class UpdateAttendanceDto
{
    /// <summary>
    /// Attendance status
    /// </summary>
    [Required]
    public AttendanceStatus Status { get; set; }
    
    /// <summary>
    /// Notes about the attendance
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// Parameters for filtering enrollments
/// </summary>
public class EnrollmentFilterParams
{
    /// <summary>
    /// Filter by course ID
    /// </summary>
    public Guid? CourseId { get; set; }
    
    /// <summary>
    /// Filter by dancer ID
    /// </summary>
    public Guid? DancerId { get; set; }
    
    /// <summary>
    /// Filter by enrollment status
    /// </summary>
    public EnrollmentStatus? Status { get; set; }
    
    /// <summary>
    /// Filter by payment status
    /// </summary>
    public PaymentStatus? PaymentStatus { get; set; }
    
    /// <summary>
    /// Include only enrollments after this date
    /// </summary>
    public DateTime? EnrollmentDateFrom { get; set; }
    
    /// <summary>
    /// Include only enrollments before this date
    /// </summary>
    public DateTime? EnrollmentDateTo { get; set; }
    
    /// <summary>
    /// Search term to filter by dancer name
    /// </summary>
    public string? SearchTerm { get; set; }
    
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