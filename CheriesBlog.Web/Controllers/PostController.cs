using System.ComponentModel;
using AutoMapper;
using CheriesBlog.Domain.Dtos;
using CheriesBlog.Domain.Interface;
using CheriesBlog.Domain.Models;
using CheriesBlog.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CheriesBlog.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostToEditDto>().ReverseMap();
            }));
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                CommentDto = new CommentDto
                {
                    PostId = id,
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(CommentDto comment)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { id = comment.PostId });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            bool result = await _postRepository.AddCommentToPostAsync(comment);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to add comment.");
                return RedirectToAction("Details", new { id = comment.PostId });

            }
            return RedirectToAction("Details", new { id = comment.PostId });
        }

        [HttpGet]
        public IActionResult AddPost()
        {
            ViewData["Title"] = "New Post";
            return View(new PostToAddDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPost(PostToAddDto postDto)
        {
            if (ModelState.IsValid)
            {
                int postId = await _postRepository.AddPostAsync(postDto);
                return RedirectToAction("Details", new { id = postId });
            }
            return View(postDto);
        }

        [HttpGet]
        public async Task<ActionResult> EditPost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit Post";
            PostToEditDto postDto = _mapper.Map<PostToEditDto>(post);
            return View(postDto);
        }


        [HttpPost]
        public async Task<ActionResult> EditPost(PostToEditDto postDto)
        {
            if (ModelState.IsValid)
            {
                if (await _postRepository.UpdatePostAsync(postDto))
                {
                    return RedirectToAction("Details", new { id = postDto.Id });
                }
                ModelState.AddModelError(string.Empty, "Failed to update post.");
            }
            return View(postDto);
        }

        [HttpPost]
        public async Task<ActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            bool result = await _postRepository.DeletePostAsync(id);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete post.");
                return RedirectToAction("Details", new { id });
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
