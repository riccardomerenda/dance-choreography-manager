using DanceChoreographyManager.Shared.Models.Base;

namespace DanceChoreographyManager.Services.Dancer.Models;

/// <summary>
/// Represents a dance style associated with a dancer
/// </summary>
public class DancerStyle : EntityBase
{
    /// <summary>
    /// Reference to the dancer
    /// </summary>
    public Guid DancerId { get; set; }
    
    /// <summary>
    /// The dancer
    /// </summary>
    public Dancer? Dancer { get; set; }
    
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
    public int YearsOfExperience { get; set; }
    
    /// <summary>
    /// Additional notes about the dancer's experience with this style
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Dance styles available in the system
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
/// Proficiency levels for dance styles
/// </summary>
public enum ProficiencyLevel
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
    /// Expert level
    /// </summary>
    Expert = 3
}