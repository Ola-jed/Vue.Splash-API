using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Data.Context
{
    public class SplashContext: IdentityDbContext<ApplicationUser>
    {
        public SplashContext(DbContextOptions<SplashContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasMany(a => a.Photos)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Photo> Photos { get; set; }
    }
}