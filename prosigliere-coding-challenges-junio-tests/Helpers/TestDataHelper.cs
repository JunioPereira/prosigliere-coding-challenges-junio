using prosigliere_coding_challenges_junio.DTOs;
using prosigliere_coding_challenges_junio.Models;

namespace prosigliere_coding_challenges_junio_tests.Helpers
{
    public static class TestDataHelper
    {
        public static BlogPost CreateSampleBlogPost(string title = "Sample Post", string content = "Sample content")
        {
            return new BlogPost
            {
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static Comment CreateSampleComment(int blogPostId, string content = "Sample comment", string author = "Sample Author")
        {
            return new Comment
            {
                BlogPostId = blogPostId,
                Content = content,
                Author = author,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static CreateBlogPostDto CreateValidBlogPostDto(string title = "Test Post", string content = "Test content")
        {
            return new CreateBlogPostDto
            {
                Title = title,
                Content = content
            };
        }

        public static CreateCommentDto CreateValidCommentDto(string content = "Test comment", string author = "Test Author")
        {
            return new CreateCommentDto
            {
                Content = content,
                Author = author
            };
        }

        public static CreateBlogPostDto CreateInvalidBlogPostDto()
        {
            return new CreateBlogPostDto
            {
                Title = "", // Invalid - empty title
                Content = "" // Invalid - empty content
            };
        }

        public static CreateCommentDto CreateInvalidCommentDto()
        {
            return new CreateCommentDto
            {
                Content = "", // Invalid - empty content
                Author = "" // Invalid - empty author
            };
        }

        public static List<BlogPost> CreateMultipleBlogPosts(int count = 3)
        {
            var posts = new List<BlogPost>();
            for (int i = 1; i <= count; i++)
            {
                posts.Add(new BlogPost
                {
                    Title = $"Post {i}",
                    Content = $"Content for post {i}",
                    CreatedAt = DateTime.UtcNow.AddDays(-i),
                    UpdatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }
            return posts;
        }

        public static List<Comment> CreateMultipleComments(int blogPostId, int count = 3)
        {
            var comments = new List<Comment>();
            for (int i = 1; i <= count; i++)
            {
                comments.Add(new Comment
                {
                    BlogPostId = blogPostId,
                    Content = $"Comment {i}",
                    Author = $"Author {i}",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-i)
                });
            }
            return comments;
        }
    }
}
