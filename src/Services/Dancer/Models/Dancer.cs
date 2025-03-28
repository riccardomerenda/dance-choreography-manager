using DanceChoreographyManager.Shared.Models.Base;

namespace DanceChoreographyManager.Services.Dancer.Models;

/// <summary>
/// Represents a dancer in the system
/// </summary>
public class Dancer : EntityBase
{
    /// <summary>
    /// First name of the dancer
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Last name of the dancer
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
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
    /// Any medical conditions or notes
    /// </summary>
    public string? MedicalNotes { get; set; }
    
    /// <summary>
    /// Additional notes about the dancer
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Date when the dancer joined
    /// </summary>
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Whether the dancer is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// List of dance styles the dancer is proficient in
    /// </summary>
    public List<DancerStyle> DanceStyles { get; set; } = new();
    
    /// <summary>
    /// Full name of the dancer
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
    
    /// <summary>
    /// Age of the dancer, calculated from date of birth
    /// </summary>
    public int? Age => DateOfBirth.HasValue 
        ? (int)((DateTime.UtcNow - DateOfBirth.Value).TotalDays / 365.25) 
        : null;
}

/// <summary>
/// Gender options for dancers
/// </summary>
public enum Gender
{
    /// <summary>
    /// Not specified
    /// </summary>
    NotSpecified = 0,
    
    /// <summary>
    /// Male
    /// </summary>
    Male = 1,
    
    /// <summary>
    /// Female
    /// </summary>
    Female = 2,
    
    /// <summary>
    /// Non-binary
    /// </summary>
    NonBinary = 3,
    
    /// <summary>
    /// Other
    /// </summary>
    Other = 4
}

/// <summary>
/// Experience level of a dancer
/// </summary>
public enum ExperienceLevel
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
    /// Professional level
    /// </summary>
    Professional = 3,
    
    /// <summary>
    /// Instructor level
    /// </summary>
    Instructor = 4
}