using Microsoft.AspNetCore.Identity;

namespace DanceChoreographyManager.Services.Authentication.Models;

public class ApplicationUser : IdentityUser
{
    // Basic profile information
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    // User preferences
    public bool DarkModeEnabled { get; set; }
    
    // Audit information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    // Helper property
    public string DisplayName => string.IsNullOrEmpty(FirstName) 
        ? UserName ?? "User" 
        : $"{FirstName} {LastName}";
}