using System;

namespace CheriesBlog.Domain.Dtos;

public class PostToAddDto
{
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Content { get; set; } = "";
    public string ImageUrl { get; set; } = "";
}
