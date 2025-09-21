using System.ComponentModel.DataAnnotations;

namespace prosigliere_coding_challenges_junio.DTOs
{
    public class CreateBlogPostDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(5000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
    }

    public class BlogPostSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CommentCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BlogPostDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
