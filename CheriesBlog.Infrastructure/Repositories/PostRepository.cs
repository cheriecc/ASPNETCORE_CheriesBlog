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

    public async Task<BlogPost> GetPostByIdAsync(int id)
    {
        var post = await _dbContext.BlogPosts.Include(p => p.Author).Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID {id} was not found.");
        }
        return post;

    }

    public async Task<IEnumerable<BlogPost>> GetAllPostsAsync()
    {
        var posts = await _dbContext.BlogPosts.Include(p => p.Author).Include(p => p.Comments).ToListAsync();
        // if (posts == null || posts.Count == 0)
        // {
        //     throw new InvalidOperationException("No posts found.");
        // }
        return posts;
    }

    public async Task<IEnumerable<BlogPost>> GetPostsByUserIdAsync()
    {
        string UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");
        var posts = await _dbContext.BlogPosts.Include(p => p.Author).Include(p => p.Comments).Where(p => p.AuthorId == Convert.ToInt32(UserId)).ToListAsync();
        if (posts == null || posts.Count == 0)
        {
            throw new InvalidOperationException($"No posts found for user with ID {UserId}.");
        }
        return posts;
    }
    public async Task<int> AddPostAsync(PostToAddDto postDto)
    {
        string userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new InvalidOperationException("User is not authenticated.");
        int? userId = int.TryParse(userIdString, out var parsedUserId) ? parsedUserId : null;

        BlogPost post = new()
        {
            Title = postDto.Title,
            Content = postDto.Content,
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            Subtitle = postDto.Subtitle,
            ImageUrl = postDto.ImageUrl,
            AuthorId = parsedUserId
                         ,
        };
        await _dbContext.BlogPosts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
        return post.Id;
    }

    public async Task<bool> UpdatePostAsync(PostToEditDto postDto)
    {
        // Retrieve the current user's ID from the claims
        string userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");
        int? userId = int.TryParse(userIdString, out var parsedUserId) ? parsedUserId : null;

        // Find the post by ID
        BlogPost post = await _dbContext.BlogPosts.FirstOrDefaultAsync(p => p.Id == postDto.Id)
            ?? throw new InvalidOperationException($"Post with ID {postDto.Id} was not found.");

        // Ensure the post belongs to the current user
        if (post.AuthorId != parsedUserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to edit this post.");
        }

        // Update the post properties
        post.Title = postDto.Title;
        post.Subtitle = postDto.Subtitle;
        post.Content = postDto.Content;
        post.ImageUrl = postDto.ImageUrl;
        //post.UpdatedAt = DateTime.UtcNow;

        // Save changes to the database
        _dbContext.BlogPosts.Update(post);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _dbContext.BlogPosts.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new InvalidOperationException($"Post with ID {id} was not found.");
        _dbContext.BlogPosts.Remove(post);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        List<Comment> comments = await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
        if (comments == null || comments.Count == 0)
        {
            throw new InvalidOperationException($"No comments found for post with ID {postId}.");
        }
        return comments;
    }
    public async Task<bool> AddCommentToPostAsync(CommentDto comment)
    {
        // Retrieve the current user's ID from the claims
        string userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");
        int? userId = int.TryParse(userIdString, out var parsedUserId) ? parsedUserId : null;

        var newComment = new Comment
        {
            Text = comment.Text,
            CommenterId = parsedUserId,
            PostId = comment.PostId,
        };
        await _dbContext.Comments.AddAsync(newComment);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> DeleteCommentFromPostAsync(int commentId)
    {
        // Retrieve the current user's ID from the claims
        string userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User is not authenticated.");
        int? userId = int.TryParse(userIdString, out var parsedUserId) ? parsedUserId : null;

        // Retrieve the comment
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId)
            ?? throw new InvalidOperationException($"Comment with ID {commentId} was not found.");
        
        // Retrieve the BlogPost
        var post = await _dbContext.BlogPosts.FirstOrDefaultAsync(p => p.Id == comment.PostId)
            ?? throw new InvalidOperationException($"Post with ID {comment.PostId} was not found.");
        if (comment.CommenterId != userId && comment.CommenterId != post.AuthorId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this comment.");
        }
        _dbContext.Comments.Remove(comment);
        int result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

}
