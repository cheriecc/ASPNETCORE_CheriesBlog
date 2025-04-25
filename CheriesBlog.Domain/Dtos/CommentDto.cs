using System;

namespace CheriesBlog.Domain.Dtos;

public class CommentDto
{
    public string Content { get; set; } = "";
    public int PostId { get; set; }
}
