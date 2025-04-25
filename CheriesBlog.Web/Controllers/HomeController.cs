using CheriesBlog.Domain.Interface;
using CheriesBlog.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheriesBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;
        public HomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        // GET: HomeController
        public async Task<ActionResult> Index()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            if (posts == null || posts.Count() == 0)
            {
                return View(new List<Post>());
            }
            return View(posts);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

    }
}
