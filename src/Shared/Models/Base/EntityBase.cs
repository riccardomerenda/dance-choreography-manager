namespace DanceChoreographyManager.Shared.Models.Base;

/// <summary>
/// Base class for all entities in the system
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// When the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Who created the entity
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// When the entity was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
    
    /// <summary>
    /// Who last modified the entity
    /// </summary>
    public string? LastModifiedBy { get; set; }
    
    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; }
}