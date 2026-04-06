using System;

namespace CheriesBlog.Domain.Dtos;

public class PostToEditDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
