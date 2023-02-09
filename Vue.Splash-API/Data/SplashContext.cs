using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Data;

public class SplashContext : DbContext
{
    public SplashContext(DbContextOptions<SplashContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>(user =>
        {
            user.HasMany(a => a.Photos)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override int SaveChanges()
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyCreatedAtToNewEntities()
    {
        var entityEntries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added);
        foreach (var entityEntry in entityEntries)
        {
            ((Model)entityEntry.Entity).CreatedAt = DateTime.Now;
        }
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; } = null!;
    public DbSet<PasswordReset> PasswordResets { get; set; } = null!;
    public DbSet<EmailVerification> EmailVerifications { get; set; } = null!;
}