using System;
using Microsoft.AspNetCore.Identity;

namespace CheriesBlog.Domain.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Post> Posts { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];
}
