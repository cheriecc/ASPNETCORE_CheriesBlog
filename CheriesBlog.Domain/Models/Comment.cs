using System;

namespace CheriesBlog.Domain.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public string CommenterId { get; set; } = "";
    public int PostId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public User? Commenter { get; set; }

}
