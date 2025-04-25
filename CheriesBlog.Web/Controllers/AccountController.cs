using CheriesBlog.Application.Services;
using CheriesBlog.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CheriesBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        public AccountController(AuthService authService)
        {
            _authService = authService;
        }
        // GET: AccountController
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginDto);
            }
            var result = await _authService.LoginUserAsync(userLoginDto);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!string.IsNullOrEmpty(result.FailureReason))
            {
                ModelState.AddModelError(string.Empty, result.FailureReason);
            }
            return View(userLoginDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(userRegistrationDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(userRegistrationDto);
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutUserAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
