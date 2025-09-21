using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prosigliere_coding_challenges_junio.Data;
using prosigliere_coding_challenges_junio.DTOs;
using prosigliere_coding_challenges_junio.Models;

namespace prosigliere_coding_challenges_junio.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class BlogPostsController : ControllerBase
    {
        private readonly BlogContext _context;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(BlogContext context, ILogger<BlogPostsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all blog posts with their comment counts
        /// </summary>
        /// <returns>List of blog posts with comment counts</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BlogPostSummaryDto>), 200)]
        public async Task<ActionResult<IEnumerable<BlogPostSummaryDto>>> GetBlogPosts()
        {
            try
            {
                var blogPosts = await _context.BlogPosts
                    .Include(bp => bp.Comments)
                    .Select(bp => new BlogPostSummaryDto
                    {
                        Id = bp.Id,
                        Title = bp.Title,
                        CommentCount = bp.Comments.Count,
                        CreatedAt = bp.CreatedAt,
                        UpdatedAt = bp.UpdatedAt
                    })
                    .OrderByDescending(bp => bp.CreatedAt)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} blog posts", blogPosts.Count);
                return Ok(blogPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blog posts");
                return StatusCode(500, "An error occurred while retrieving blog posts");
            }
        }

        /// <summary>
        /// Create a new blog post
        /// </summary>
        /// <param name="createBlogPostDto">Blog post data</param>
        /// <returns>Created blog post</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BlogPostDetailDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BlogPostDetailDto>> CreateBlogPost([FromBody] CreateBlogPostDto createBlogPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var blogPost = new BlogPost
                {
                    Title = createBlogPostDto.Title.Trim(),
                    Content = createBlogPostDto.Content.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();

                var result = new BlogPostDetailDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    CreatedAt = blogPost.CreatedAt,
                    UpdatedAt = blogPost.UpdatedAt,
                    Comments = new List<CommentDto>()
                };

                _logger.LogInformation("Created blog post with ID {BlogPostId}", blogPost.Id);
                return Created($"/api/posts/{blogPost.Id}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blog post");
                return StatusCode(500, "An error occurred while creating the blog post");
            }
        }

        /// <summary>
        /// Get a specific blog post by ID with all comments
        /// </summary>
        /// <param name="id">Blog post ID</param>
        /// <returns>Blog post with comments</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlogPostDetailDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BlogPostDetailDto>> GetBlogPost(int id)
        {
            try
            {
                var blogPost = await _context.BlogPosts
                    .Include(bp => bp.Comments)
                    .FirstOrDefaultAsync(bp => bp.Id == id);

                if (blogPost == null)
                {
                    _logger.LogWarning("Blog post with ID {BlogPostId} not found", id);
                    return NotFound($"Blog post with ID {id} not found");
                }

                var result = new BlogPostDetailDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    CreatedAt = blogPost.CreatedAt,
                    UpdatedAt = blogPost.UpdatedAt,
                    Comments = blogPost.Comments
                        .OrderBy(c => c.CreatedAt)
                        .Select(c => new CommentDto
                        {
                            Id = c.Id,
                            Content = c.Content,
                            Author = c.Author,
                            CreatedAt = c.CreatedAt
                        })
                        .ToList()
                };

                _logger.LogInformation("Retrieved blog post with ID {BlogPostId}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blog post with ID {BlogPostId}", id);
                return StatusCode(500, "An error occurred while retrieving the blog post");
            }
        }

        /// <summary>
        /// Add a comment to a specific blog post
        /// </summary>
        /// <param name="id">Blog post ID</param>
        /// <param name="createCommentDto">Comment data</param>
        /// <returns>Created comment</returns>
        [HttpPost("{id}/comments")]
        [ProducesResponseType(typeof(CommentDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CommentDto>> AddComment(int id, [FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var blogPost = await _context.BlogPosts.FindAsync(id);
                if (blogPost == null)
                {
                    _logger.LogWarning("Blog post with ID {BlogPostId} not found when adding comment", id);
                    return NotFound($"Blog post with ID {id} not found");
                }

                var comment = new Comment
                {
                    Content = createCommentDto.Content.Trim(),
                    Author = createCommentDto.Author.Trim(),
                    BlogPostId = id,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                var result = new CommentDto
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    Author = comment.Author,
                    CreatedAt = comment.CreatedAt
                };

                _logger.LogInformation("Added comment with ID {CommentId} to blog post {BlogPostId}", comment.Id, id);
                return Created($"/api/posts/{id}/comments/{comment.Id}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to blog post with ID {BlogPostId}", id);
                return StatusCode(500, "An error occurred while adding the comment");
            }
        }
    }
}
