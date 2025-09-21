using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using prosigliere_coding_challenges_junio.Controllers;
using prosigliere_coding_challenges_junio.Data;
using prosigliere_coding_challenges_junio.DTOs;
using prosigliere_coding_challenges_junio.Models;

namespace prosigliere_coding_challenges_junio_tests.Controllers
{
    public class BlogPostsControllerTests : IDisposable
    {
        private readonly BlogContext _context;
        private readonly Mock<ILogger<BlogPostsController>> _loggerMock;
        private readonly BlogPostsController _controller;

        public BlogPostsControllerTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BlogContext(options);
            _loggerMock = new Mock<ILogger<BlogPostsController>>();
            _controller = new BlogPostsController(_context, _loggerMock.Object);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #region GetBlogPosts Tests

        [Fact]
        public async Task GetBlogPosts_WhenNoPosts_ReturnsEmptyList()
        {
            // Act
            var result = await _controller.GetBlogPosts();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<BlogPostSummaryDto>>().Subject;
            posts.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBlogPosts_WhenPostsExist_ReturnsPostsWithCommentCounts()
        {
            // Arrange
            var blogPost1 = new BlogPost
            {
                Title = "First Post",
                Content = "Content 1",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            };

            var blogPost2 = new BlogPost
            {
                Title = "Second Post",
                Content = "Content 2",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };

            _context.BlogPosts.AddRange(blogPost1, blogPost2);
            await _context.SaveChangesAsync();

            // Add comments to first post
            _context.Comments.AddRange(
                new Comment { BlogPostId = blogPost1.Id, Content = "Comment 1", Author = "Author 1", CreatedAt = DateTime.UtcNow },
                new Comment { BlogPostId = blogPost1.Id, Content = "Comment 2", Author = "Author 2", CreatedAt = DateTime.UtcNow }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBlogPosts();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<BlogPostSummaryDto>>().Subject.ToList();
            
            posts.Should().HaveCount(2);
            posts.First().Title.Should().Be("Second Post"); // Should be ordered by CreatedAt descending
            posts.First().CommentCount.Should().Be(0);
            posts.Last().Title.Should().Be("First Post");
            posts.Last().CommentCount.Should().Be(2);
        }

        [Fact]
        public async Task GetBlogPosts_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _context.Dispose(); // Force an exception

            // Act
            var result = await _controller.GetBlogPosts();

            // Assert
            var statusResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusResult.StatusCode.Should().Be(500);
        }

        #endregion

        #region CreateBlogPost Tests

        [Fact]
        public async Task CreateBlogPost_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreateBlogPostDto
            {
                Title = "", // Invalid - empty title
                Content = "Test Content"
            };
            
            _controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _controller.CreateBlogPost(createDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region GetBlogPost Tests

        [Fact]
        public async Task GetBlogPost_WhenPostExists_ReturnsPostWithComments()
        {
            // Arrange
            var blogPost = new BlogPost
            {
                Title = "Test Post",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            var comment = new Comment
            {
                BlogPostId = blogPost.Id,
                Content = "Test Comment",
                Author = "Test Author",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBlogPost(blogPost.Id);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var postDetail = okResult.Value.Should().BeOfType<BlogPostDetailDto>().Subject;
            
            postDetail.Id.Should().Be(blogPost.Id);
            postDetail.Title.Should().Be("Test Post");
            postDetail.Content.Should().Be("Test Content");
            postDetail.Comments.Should().HaveCount(1);
            postDetail.Comments.First().Content.Should().Be("Test Comment");
            postDetail.Comments.First().Author.Should().Be("Test Author");
        }

        [Fact]
        public async Task GetBlogPost_WhenPostDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetBlogPost(999);

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be("Blog post with ID 999 not found");
        }

        [Fact]
        public async Task GetBlogPost_CommentsOrderedByCreatedAt_ReturnsOrderedComments()
        {
            // Arrange
            var blogPost = new BlogPost
            {
                Title = "Test Post",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            var comment1 = new Comment
            {
                BlogPostId = blogPost.Id,
                Content = "First Comment",
                Author = "Author 1",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            };

            var comment2 = new Comment
            {
                BlogPostId = blogPost.Id,
                Content = "Second Comment",
                Author = "Author 2",
                CreatedAt = DateTime.UtcNow.AddMinutes(-5)
            };

            _context.Comments.AddRange(comment2, comment1); // Add in reverse order
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBlogPost(blogPost.Id);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var postDetail = okResult.Value.Should().BeOfType<BlogPostDetailDto>().Subject;
            
            postDetail.Comments.Should().HaveCount(2);
            postDetail.Comments.First().Content.Should().Be("First Comment"); // Should be ordered by CreatedAt
            postDetail.Comments.Last().Content.Should().Be("Second Comment");
        }

        #endregion

        #region AddComment Tests
        [Fact]
        public async Task AddComment_WhenBlogPostDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var createCommentDto = new CreateCommentDto
            {
                Content = "Test Comment",
                Author = "Test Author"
            };

            // Act
            var result = await _controller.AddComment(999, createCommentDto);

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be("Blog post with ID 999 not found");
        }

        [Fact]
        public async Task AddComment_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var blogPost = new BlogPost
            {
                Title = "Test Post",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            var createCommentDto = new CreateCommentDto
            {
                Content = "", // Invalid - empty content
                Author = "Test Author"
            };
            
            _controller.ModelState.AddModelError("Content", "Content is required");

            // Act
            var result = await _controller.AddComment(blogPost.Id, createCommentDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion
    }
}
