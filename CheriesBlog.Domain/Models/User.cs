using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheriesBlog.Domain.Models;

[Table("user_pool")]
public class User : IdentityUser<int>
{
    [Key]
    [Column("id")]
    public override int Id { get; set; }

    [Column("email")]
    [MaxLength(100)]
    public override string? Email { get; set; }

    [Column("password")]
    [MaxLength(250)]
    [Required]
    public string Password { get; set; } = string.Empty;

    [Column("name")]
    [MaxLength(20)]
    [Required]
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}