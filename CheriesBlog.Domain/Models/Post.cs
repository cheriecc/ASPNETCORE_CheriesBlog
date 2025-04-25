using System;

namespace CheriesBlog.Domain.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Content { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = "";
    public ICollection<Comment> Comments { get; set; } = [];
    [System.Text.Json.Serialization.JsonIgnore]
    public User? Author { get; set; }
}
