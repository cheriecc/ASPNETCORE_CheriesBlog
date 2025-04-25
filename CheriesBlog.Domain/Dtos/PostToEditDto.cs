using System;

namespace CheriesBlog.Domain.Dtos;

public class PostToEditDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Content { get; set; } = "";
    public string ImageUrl { get; set; } = "";
}
