using DanceChoreographyManager.Services.Authentication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DanceChoreographyManager.Services.Authentication.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure table names with a prefix to avoid conflicts
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null && !tableName.StartsWith("Auth_"))
            {
                entity.SetTableName($"Auth_{tableName}");
            }
        }

        // Add any additional configurations here
    }
}