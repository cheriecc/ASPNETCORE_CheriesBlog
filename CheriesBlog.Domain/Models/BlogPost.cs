using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheriesBlog.Domain.Models
{
    [Table("blog_posts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        [MaxLength(250)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [Column("subtitle")]
        [MaxLength(250)]
        [Required]
        public string Subtitle { get; set; } = string.Empty;

        [Column("author_id")]
        public int AuthorId { get; set; }

        [Column("date")]
        [MaxLength(250)]
        [Required]
        public string Date { get; set; } = string.Empty;

        [Column("body")]
        [Required]
        public string Content { get; set; } = string.Empty;

        [Column("img_url")]
        [MaxLength(250)]
        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation properties
        public User Author { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
