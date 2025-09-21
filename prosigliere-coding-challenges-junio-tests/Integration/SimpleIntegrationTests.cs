using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using prosigliere_coding_challenges_junio.Data;
using prosigliere_coding_challenges_junio.DTOs;

namespace prosigliere_coding_challenges_junio_tests.Integration
{
    public class SimpleIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public SimpleIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing BlogContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<BlogContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Add a new in-memory database for testing
                    services.AddDbContext<BlogContext>(options =>
                    {
                        options.UseInMemoryDatabase($"SimpleTestDb_{Guid.NewGuid()}");
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        private async Task ClearDatabaseAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BlogContext>();
            
            context.Comments.RemoveRange(context.Comments);
            context.BlogPosts.RemoveRange(context.BlogPosts);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetPosts_ReturnsSuccessStatusCode()
        {
            // Arrange
            await ClearDatabaseAsync();

            // Act
            var response = await _client.GetAsync("/api/posts");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreatePost_WithValidData_ReturnsCreated()
        {
            // Arrange
            await ClearDatabaseAsync();
            var createDto = new CreateBlogPostDto
            {
                Title = "Simple Test Post",
                Content = "Simple test content"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/posts", createDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            
            var createdPost = await response.Content.ReadFromJsonAsync<BlogPostDetailDto>();
            createdPost.Should().NotBeNull();
            createdPost!.Title.Should().Be("Simple Test Post");
            createdPost.Content.Should().Be("Simple test content");
        }

        [Fact]
        public async Task CreatePost_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            await ClearDatabaseAsync();
            var createDto = new CreateBlogPostDto
            {
                Title = "", // Invalid
                Content = "Valid content"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/posts", createDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetNonExistentPost_ReturnsNotFound()
        {
            // Arrange
            await ClearDatabaseAsync();

            // Act
            var response = await _client.GetAsync("/api/posts/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddCommentToNonExistentPost_ReturnsNotFound()
        {
            // Arrange
            await ClearDatabaseAsync();
            var createCommentDto = new CreateCommentDto
            {
                Content = "Test comment",
                Author = "Test Author"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/posts/999/comments", createCommentDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
