using System.Security.Claims;
using CheriesBlog.Domain.Dtos;
using CheriesBlog.Domain.Interface;
using CheriesBlog.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheriesBlog.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController(IHttpContextAccessor httpContextAccessor, IPostRepository postRepository) : ControllerBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IPostRepository _postRepository = postRepository;

        [Authorize]
        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _postRepository.GetAllPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetMyPosts")]
        public async Task<IActionResult> GetMyPosts()
        {
            try
            {

                var posts = await _postRepository.GetPostsByUserIdAsync();
                if (posts == null || !posts.Any())
                {
                    return NotFound("No posts found for this user.");
                }
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(id);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost([FromBody] PostToAddDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _postRepository.AddPostAsync(postDto);
                return Ok("Post added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody] PostToEditDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _postRepository.UpdatePostAsync(postDto);
                return Ok("Post updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postRepository.DeletePostAsync(id);
                return Ok("Post deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetCommentsByPostId/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            try
            {
                var comments = await _postRepository.GetCommentsByPostIdAsync(postId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddCommentToPost")]
        public async Task<IActionResult> AddCommentToPost([FromBody] CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _postRepository.AddCommentToPostAsync(commentDto);
                return Ok("Comment added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCommentFromPost/{commentId}")]
        public async Task<IActionResult> DeleteCommentFromPost(int commentId)
        {
            try
            {
                await _postRepository.DeleteCommentFromPostAsync(commentId);
                return Ok("Comment deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
