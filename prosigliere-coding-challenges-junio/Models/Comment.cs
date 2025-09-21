using System.ComponentModel.DataAnnotations;

namespace prosigliere_coding_challenges_junio.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Author { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int BlogPostId { get; set; }
        
        public BlogPost BlogPost { get; set; } = null!;
    }
}
