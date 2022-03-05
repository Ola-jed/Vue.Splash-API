using System;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Data;

public class SplashContext : DbContext
{
    public SplashContext(DbContextOptions<SplashContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(user =>
        {
            user.HasMany(a => a.Photos)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; } = null!;
    public DbSet<PasswordReset> PasswordResets { get; set; } = null!;
    public DbSet<EmailVerification> EmailVerifications { get; set; } = null!;
}