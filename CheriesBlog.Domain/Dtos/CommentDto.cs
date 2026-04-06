using System;

namespace CheriesBlog.Domain.Dtos;

public class CommentDto
{
    public string Text { get; set; } = string.Empty;
    public int PostId { get; set; }
}
