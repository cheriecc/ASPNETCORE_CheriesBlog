using System;
using System.Security.Claims;
using CheriesBlog.Domain.Dtos;
using CheriesBlog.Domain.Interface;
using CheriesBlog.Domain.Models;
using CheriesBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CheriesBlog.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{

    private readonly BlogDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostRepository(BlogDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Post> GetPostByIdAsync(int id)
    {
        var post = await _dbContext.Posts.Include(p => p.Author).Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID {id} was not found.");
        }
        return post;

    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        var posts = await _dbContext.Posts.Include(p => p.Author).Include(p => p.Comments).ToListAsync();
        // if (posts == null || posts.Count == 0)
        // {
        //     throw new InvalidOperationException("No posts found.");
        // }
        return posts;
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync()
    {
        string UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");
        var posts = await _dbContext.Posts.Include(p => p.Author).Include(p => p.Comments).Where(p => p.UserId == UserId).ToListAsync();
        if (posts == null || posts.Count == 0)
        {
            throw new InvalidOperationException($"No posts found for user with ID {UserId}.");
        }
        return posts;
    }
    public async Task<int> AddPostAsync(PostToAddDto postDto)
    {
        Post post = new()
        {
            Title = postDto.Title,
            Content = postDto.Content,
            CreatedAt = DateTime.UtcNow,
            SubTitle = postDto.SubTitle,
            ImageUrl = postDto.ImageUrl,
            UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new InvalidOperationException("User is not authenticated."),
        };
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
        return post.Id;
    }

    public async Task<bool> UpdatePostAsync(PostToEditDto postDto)
    {
        // Retrieve the current user's ID from the claims
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");

        // Find the post by ID
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postDto.Id);

        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID {postDto.Id} was not found.");
        }

        // Ensure the post belongs to the current user
        if (post.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to edit this post.");
        }

        // Update the post properties
        post.Title = postDto.Title;
        post.SubTitle = postDto.SubTitle;
        post.Content = postDto.Content;
        post.ImageUrl = postDto.ImageUrl;
        post.UpdatedAt = DateTime.UtcNow;
        // Save changes to the database
        _dbContext.Posts.Update(post);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new InvalidOperationException($"Post with ID {id} was not found.");
        _dbContext.Posts.Remove(post);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        var comments = await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
        if (comments == null || comments.Count == 0)
        {
            throw new InvalidOperationException($"No comments found for post with ID {postId}.");
        }
        return comments;
    }
    public async Task<bool> AddCommentToPostAsync(CommentDto comment)
    {
        var newComment = new Comment
        {
            Content = comment.Content,
            CreatedAt = DateTime.UtcNow,
            CommenterId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new InvalidOperationException("User is not authenticated."),
            PostId = comment.PostId,
        };
        await _dbContext.Comments.AddAsync(newComment);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> DeleteCommentFromPostAsync(int commentId)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId)
            ?? throw new InvalidOperationException($"Comment with ID {commentId} was not found.");
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == comment.PostId)
            ?? throw new InvalidOperationException($"Post with ID {comment.PostId} was not found.");
        if (comment.CommenterId != userId && comment.CommenterId != post.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this comment.");
        }
        _dbContext.Comments.Remove(comment);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

}
