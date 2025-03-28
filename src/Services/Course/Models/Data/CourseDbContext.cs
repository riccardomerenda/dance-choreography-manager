using DanceChoreographyManager.Services.Course.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceChoreographyManager.Services.Course.Data;

public class CourseDbContext : DbContext
{
    public CourseDbContext(DbContextOptions<CourseDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Models.Course> Courses { get; set; } = null!;
    public DbSet<CourseSession> CourseSessions { get; set; } = null!;
    public DbSet<CourseEnrollment> CourseEnrollments { get; set; } = null!;
    public DbSet<SessionAttendance> SessionAttendances { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure table names with a prefix to avoid conflicts
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null && !tableName.StartsWith("Course_"))
            {
                entity.SetTableName($"Course_{tableName}");
            }
        }
        
        // Configure Course entity
        modelBuilder.Entity<Models.Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.InstructorName).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            
            // Create index on name for faster searching
            entity.HasIndex(e => e.Name);
            
            // Create index on start and end dates for filtering
            entity.HasIndex(e => new { e.StartDate, e.EndDate });
        });
        
        // Configure CourseSession entity
        modelBuilder.Entity<CourseSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            
            // Configure relationship with Course
            entity.HasOne(e => e.Course)
                .WithMany(c => c.Sessions)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Create index on start and end times for filtering
            entity.HasIndex(e => new { e.StartDateTime, e.EndDateTime });
        });
        
        // Configure CourseEnrollment entity
        modelBuilder.Entity<CourseEnrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DancerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DancerEmail).HasMaxLength(255);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.AmountPaid).HasPrecision(10, 2);
            
            // Configure relationship with Course
            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Create unique constraint for the dancer-course combination
            entity.HasIndex(e => new { e.CourseId, e.DancerId }).IsUnique();
        });
        
        // Configure SessionAttendance entity
        modelBuilder.Entity<SessionAttendance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DancerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            // Configure relationship with CourseSession
            entity.HasOne(e => e.Session)
                .WithMany(s => s.Attendances)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Create unique constraint for the dancer-session combination
            entity.HasIndex(e => new { e.SessionId, e.DancerId }).IsUnique();
        });
    }
}