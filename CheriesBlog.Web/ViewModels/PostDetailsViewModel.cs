using System;
using CheriesBlog.Domain.Dtos;
using CheriesBlog.Domain.Models;

namespace CheriesBlog.Web.ViewModels;

public class PostDetailsViewModel
{
    public required BlogPost Post { get; set; }
    public required CommentDto CommentDto { get; set; }
}
