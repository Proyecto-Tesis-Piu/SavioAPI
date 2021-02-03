using Microsoft.EntityFrameworkCore;
using MonetaAPI.Models;

namespace MonetaAPI.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }
        public DbSet<BlogArticle> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogArticle>().ToTable("Blog_Articles");
        }
    }
}
