using Microsoft.EntityFrameworkCore;
using MonetaAPI.Models;

namespace MonetaAPI.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) :base(options)
        { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("User").HasKey(u => u.Id);
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
        }
    }
}
