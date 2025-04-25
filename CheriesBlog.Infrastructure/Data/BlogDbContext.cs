using System;
using CheriesBlog.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CheriesBlog.Infrastructure.Data;

public class BlogDbContext : IdentityDbContext<User>
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("CheriesBlog");
        // Configure the User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.Author)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes for posts when a user is deleted
        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.Commenter)
            .HasForeignKey(c => c.CommenterId)
            .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes for comments when a user is deleted

        //Configure the Post entity
        modelBuilder.Entity<Post>()
            .HasKey(p => p.Id);
        // modelBuilder.Entity<Post>()
        //     .HasMany(p => p.Comments)
        //     .WithOne(c => c.Post)
        //     .HasForeignKey(c => c.PostId)
        //     .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes for comments when a post is deleted

        // Configure the Comment entity
        modelBuilder.Entity<Comment>()
        .HasKey(c => c.Id);
        // modelBuilder.Entity<Comment>()
        //     .HasOne(c => c.Post)
        //     .WithMany(p => p.Comments)
        //     .HasForeignKey(c => c.PostId)
        //     .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for comments when a post is deleted
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Commenter)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CommenterId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for users

    }



}
