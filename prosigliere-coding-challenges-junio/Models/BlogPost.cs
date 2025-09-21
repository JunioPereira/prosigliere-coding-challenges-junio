using System.ComponentModel.DataAnnotations;

namespace prosigliere_coding_challenges_junio.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(5000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
