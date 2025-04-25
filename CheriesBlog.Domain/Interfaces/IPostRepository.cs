using System;
using CheriesBlog.Domain.Dtos;
using CheriesBlog.Domain.Models;

namespace CheriesBlog.Domain.Interface;

public interface IPostRepository
{

    Task<Post> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<IEnumerable<Post>> GetPostsByUserIdAsync();
    Task<int> AddPostAsync(PostToAddDto post);
    Task<bool> UpdatePostAsync(PostToEditDto post);
    Task<bool> DeletePostAsync(int id);
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    Task<bool> AddCommentToPostAsync(CommentDto comment);
    Task<bool> DeleteCommentFromPostAsync(int commentId);

}
