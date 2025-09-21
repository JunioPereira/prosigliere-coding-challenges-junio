using Microsoft.EntityFrameworkCore;
using prosigliere_coding_challenges_junio.Data;
using prosigliere_coding_challenges_junio.Models;

namespace prosigliere_coding_challenges_junio_tests.TestContext
{
    public class TestBlogContext : BlogContext
    {
        public TestBlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure BlogPost entity
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(5000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // Configure one-to-many relationship
                entity.HasMany(e => e.Comments)
                      .WithOne(e => e.BlogPost)
                      .HasForeignKey(e => e.BlogPostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Comment entity
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.BlogPostId).IsRequired();
            });

            // No seed data for testing - we want clean tests
        }
    }
}
