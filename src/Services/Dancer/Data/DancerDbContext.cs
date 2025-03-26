using DanceChoreographyManager.Services.Dancer.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceChoreographyManager.Services.Dancer.Data;

public class DancerDbContext : DbContext
{
    public DancerDbContext(DbContextOptions<DancerDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Models.Dancer> Dancers { get; set; } = null!;
    public DbSet<DancerStyle> DancerStyles { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure table names with a prefix to avoid conflicts
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null && !tableName.StartsWith("Dancer_"))
            {
                entity.SetTableName($"Dancer_{tableName}");
            }
        }
        
        // Configure Dancer entity
        modelBuilder.Entity<Models.Dancer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.EmergencyContactName).HasMaxLength(200);
            entity.Property(e => e.EmergencyContactPhone).HasMaxLength(20);
            entity.Property(e => e.MedicalNotes).HasMaxLength(1000);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            
            // Create index on name for faster searching
            entity.HasIndex(e => new { e.FirstName, e.LastName });
            
            // Create index on email for faster searching and uniqueness
            entity.HasIndex(e => e.Email).IsUnique(true);
        });
        
        // Configure DancerStyle entity
        modelBuilder.Entity<DancerStyle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            // Configure relationship with Dancer
            entity.HasOne(e => e.Dancer)
                .WithMany(d => d.DanceStyles)
                .HasForeignKey(e => e.DancerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}