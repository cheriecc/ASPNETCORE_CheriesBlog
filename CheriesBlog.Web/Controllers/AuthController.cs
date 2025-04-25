using CheriesBlog.Application.Services;
using CheriesBlog.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheriesBlog.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        private readonly AuthService _authService = authService;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterUserAsync(userDto);
            if (result.Succeeded)
            {
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginUserAsync(userDto);
            if (result.Succeeded)
            {
                var user = await _authService.GetUserByEmailAsync(userDto.Username);

                if (user == null)
                {
                    return Unauthorized(new { message = "User does not exist." });
                }

                var token = _authService.GenerateJwtToken(user.Id);
                return Ok(token);
            }
            if (result.IsLockedOut)
            {
                return Unauthorized(new { message = "User account is locked." });
            }
            if (result.IsNotAllowed)
            {
                return Unauthorized(new { message = "User is not allowed to log in." });
            }

            return Unauthorized("Invalid username or password");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.ResetPassword(userDto);
            return Ok("Password reset successfully.");
        }

    }

}
