using Microsoft.EntityFrameworkCore;
using MonetaAPI.Models;

namespace MonetaAPI.Data
{
    public class CountriesContext : DbContext
    {
        public CountriesContext(DbContextOptions<CountriesContext> options) : base(options) { }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<State>().ToTable("State");
        }
    }
}

