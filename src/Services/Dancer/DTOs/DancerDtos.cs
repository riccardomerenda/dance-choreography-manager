using System.ComponentModel.DataAnnotations;
using DanceChoreographyManager.Services.Dancer.Models;

namespace DanceChoreographyManager.Services.Dancer.DTOs;

/// <summary>
/// DTO for creating a new dancer
/// </summary>
public class CreateDancerDto
{
    /// <summary>
    /// First name of the dancer
    /// </summary>
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Last name of the dancer
    /// </summary>
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Email address of the dancer
    /// </summary>
    [EmailAddress]
    [StringLength(255)]
    public string? Email { get; set; }
    
    /// <summary>
    /// Phone number of the dancer
    /// </summary>
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Date of birth of the dancer
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// Gender of the dancer
    /// </summary>
    public Gender Gender { get; set; } = Gender.NotSpecified;
    
    /// <summary>
    /// Height of the dancer in centimeters
    /// </summary>
    [Range(50, 250)]
    public int? HeightCm { get; set; }
    
    /// <summary>
    /// Weight of the dancer in kilograms
    /// </summary>
    [Range(20, 200)]
    public int? WeightKg { get; set; }
    
    /// <summary>
    /// Experience level of the dancer
    /// </summary>
    public ExperienceLevel ExperienceLevel { get; set; } = ExperienceLevel.Beginner;
    
    /// <summary>
    /// Emergency contact name
    /// </summary>
    [StringLength(200)]
    public string? EmergencyContactName { get; set; }
    
    /// <summary>
    /// Emergency contact phone number
    /// </summary>
    [StringLength(20)]
    public string? EmergencyContactPhone { get; set; }
    
    /// <summary>
    /// Any medical conditions or notes
    /// </summary>
    [StringLength(1000)]
    public string? MedicalNotes { get; set; }
    
    /// <summary>
    /// Additional notes about the dancer
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Dance styles the dancer is proficient in
    /// </summary>
    public List<DancerStyleDto>? DanceStyles { get; set; }
}

/// <summary>
/// DTO for updating an existing dancer
/// </summary>
public class UpdateDancerDto
{
    /// <summary>
    /// First name of the dancer
    /// </summary>
    [StringLength(100)]
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Last name of the dancer
    /// </summary>
    [StringLength(100)]
    public string? LastName { get; set; }
    
    /// <summary>
    /// Email address of the dancer
    /// </summary>
    [EmailAddress]
    [StringLength(255)]
    public string? Email { get; set; }
    
    /// <summary>
    /// Phone number of the dancer
    /// </summary>
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Date of birth of the dancer
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// Gender of the dancer
    /// </summary>
    public Gender? Gender { get; set; }
    
    /// <summary>
    /// Height of the dancer in centimeters
    /// </summary>
    [Range(50, 250)]
    public int? HeightCm { get; set; }
    
    /// <summary>
    /// Weight of the dancer in kilograms
    /// </summary>
    [Range(20, 200)]
    public int? WeightKg { get; set; }
    
    /// <summary>
    /// Experience level of the dancer
    /// </summary>
    public ExperienceLevel? ExperienceLevel { get; set; }
    
    /// <summary>
    /// Emergency contact name
    /// </summary>
    [StringLength(200)]
    public string? EmergencyContactName { get; set; }
    
    /// <summary>
    /// Emergency contact phone number
    /// </summary>
    [StringLength(20)]
    public string? EmergencyContactPhone { get; set; }
    
    /// <summary>
    /// Any medical conditions or notes
    /// </summary>
    [StringLength(1000)]
    public string? MedicalNotes { get; set; }
    
    /// <summary>
    /// Additional notes about the dancer
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Whether the dancer is active
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// DTO for dancer dance style
/// </summary>
public class DancerStyleDto
{
    /// <summary>
    /// The dance style
    /// </summary>
    public DanceStyle Style { get; set; }
    
    /// <summary>
    /// Proficiency level in this style
    /// </summary>
    public ProficiencyLevel ProficiencyLevel { get; set; }
    
    /// <summary>
    /// Years of experience in this style
    /// </summary>
    [Range(0, 50)]
    public int YearsOfExperience { get; set; }
    
    /// <summary>
    /// Additional notes about the dancer's experience with this style
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for dancer response
/// </summary>
public class DancerDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// First name of the dancer
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Last name of the dancer
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Full name of the dancer
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Email address of the dancer
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Phone number of the dancer
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Date of birth of the dancer
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// Age of the dancer
    /// </summary>
    public int? Age { get; set; }
    
    /// <summary>
    /// Gender of the dancer
    /// </summary>
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Height of the dancer in centimeters
    /// </summary>
    public int? HeightCm { get; set; }
    
    /// <summary>
    /// Weight of the dancer in kilograms
    /// </summary>
    public int? WeightKg { get; set; }
    
    /// <summary>
    /// Experience level of the dancer
    /// </summary>
    public ExperienceLevel ExperienceLevel { get; set; }
    
    /// <summary>
    /// Emergency contact name
    /// </summary>
    public string? EmergencyContactName { get; set; }
    
    /// <summary>
    /// Emergency contact phone number
    /// </summary>
    public string? EmergencyContactPhone { get; set; }
    
    /// <summary>
    /// Date when the dancer joined
    /// </summary>
    public DateTime JoinedDate { get; set; }
    
    /// <summary>
    /// Whether the dancer is active
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Dance styles the dancer is proficient in
    /// </summary>
    public List<DancerStyleDto> DanceStyles { get; set; } = new();
    
    /// <summary>
    /// When the dancer record was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When the dancer record was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
}

/// <summary>
/// Parameters for filtering dancers
/// </summary>
public class DancerFilterParams
{
    /// <summary>
    /// Search term to filter by name
    /// </summary>
    public string? SearchTerm { get; set; }
    
    /// <summary>
    /// Filter by gender
    /// </summary>
    public Gender? Gender { get; set; }
    
    /// <summary>
    /// Filter by minimum experience level
    /// </summary>
    public ExperienceLevel? MinExperienceLevel { get; set; }
    
    /// <summary>
    /// Filter by dance style
    /// </summary>
    public DanceStyle? DanceStyle { get; set; }
    
    /// <summary>
    /// Include only active dancers
    /// </summary>
    public bool? IsActive { get; set; }
    
    /// <summary>
    /// Filter by age range minimum
    /// </summary>
    [Range(0, 120)]
    public int? MinAge { get; set; }
    
    /// <summary>
    /// Filter by age range maximum
    /// </summary>
    [Range(0, 120)]
    public int? MaxAge { get; set; }
    
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