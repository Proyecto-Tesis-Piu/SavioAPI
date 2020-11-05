using Microsoft.EntityFrameworkCore;
using MonetaAPI.Models;

namespace MonetaAPI.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) :base(options)
        { }

        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("User").HasKey(u => u.Id);
        }
    }
}
