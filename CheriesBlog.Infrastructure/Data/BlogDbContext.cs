using CheriesBlog.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CheriesBlog.Infrastructure.Data;

public class BlogDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map Identity tables to your existing table names
        modelBuilder.Entity<User>().ToTable("user_pool");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");

        // User -> BlogPosts (one-to-many)
        modelBuilder.Entity<BlogPost>()
            .HasOne(bp => bp.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(bp => bp.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // BlogPost -> Comments (one-to-many)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.SubjectPost)
            .WithMany(bp => bp.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Comments (one-to-many)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Commenter)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CommenterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint on BlogPost.Title (mirrors unique=True)
        modelBuilder.Entity<BlogPost>()
            .HasIndex(bp => bp.Title)
            .IsUnique();

        // Unique constraint on User.Email (mirrors unique=True)
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }


}
