using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheriesBlog.Domain.Models;

[Table("comments")]
public class Comment
{
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [Required]
    public string Text { get; set; } = string.Empty;

    [Column("post_id")]
    public int PostId { get; set; }

    [Column("commenter_id")]
    public int CommenterId { get; set; }

    // Navigation properties
    public BlogPost SubjectPost { get; set; } = null!;
    public User Commenter { get; set; } = null!;
}
