using Microsoft.EntityFrameworkCore;
using prosigliere_coding_challenges_junio.Models;

namespace prosigliere_coding_challenges_junio.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            // Seed some initial data
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost
                {
                    Id = 1,
                    Title = "Welcome to Our Blog",
                    Content = "This is the first blog post on our platform. We're excited to share our thoughts and ideas with you!",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Getting Started with ASP.NET Core",
                    Content = "ASP.NET Core is a powerful framework for building web applications and APIs. In this post, we'll explore the basics of creating a RESTful API.",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    Content = "Great introduction! Looking forward to more posts.",
                    Author = "John Doe",
                    BlogPostId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Comment
                {
                    Id = 2,
                    Content = "Very helpful tutorial. Thanks for sharing!",
                    Author = "Jane Smith",
                    BlogPostId = 2,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Comment
                {
                    Id = 3,
                    Content = "Could you cover more advanced topics in future posts?",
                    Author = "Mike Johnson",
                    BlogPostId = 2,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            );
        }
    }
}
