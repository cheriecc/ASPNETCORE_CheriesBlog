using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CheriesBlog.Domain.Dtos;
using CheriesBlog.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CheriesBlog.Application.Services;

public class AuthService
{

    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(userDto.Password));
        }
        if (userDto.Password != userDto.PasswordConfirmation)
        {
            throw new ArgumentException("Password and confirmation password do not match.", nameof(userDto.Password));
        }
        if (await _userManager.FindByEmailAsync(userDto.Email) != null)
        {
            throw new InvalidOperationException($"User with email {userDto.Email} already exists.");
        }

        var user = new User
        {
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            UserName = userDto.Email,
            CreatedAt = DateTime.UtcNow
        };

        return await _userManager.CreateAsync(user, userDto.Password);
    }

    public async Task<LogInResult> LoginUserAsync(UserLoginDto userDto)
    {
        var user = await _userManager.FindByNameAsync(userDto.Username);
        if (user == null)
        {
            return new LogInResult
            {
                Succeeded = false,
                FailureReason = "The username does not exist."
            };
        }
        var result = await _signInManager.PasswordSignInAsync(userDto.Username, userDto.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return new LogInResult { Succeeded = true };
        }
        if (result.IsLockedOut)
        {
            return new LogInResult
            {
                Succeeded = false,
                IsLockedOut = true,
                FailureReason = "User account is locked out."
            };
        }
        if (result.IsNotAllowed)
        {
            return new LogInResult
            {
                Succeeded = false,
                IsNotAllowed = true,
                FailureReason = "User is not allowed to log in."
            };
        }
        return new LogInResult
        {
            Succeeded = false,
            FailureReason = "Incorrect password."
        };
    }

    public async Task ResetPassword(UserResetPasswordDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(userDto.Password));
        }

        if (userDto.Password != userDto.PasswordConfirmation)
        {
            throw new ArgumentException("Password and confirmation password do not match.", nameof(userDto.Password));
        }

        var user = await _userManager.FindByNameAsync(userDto.Username);
        if (user == null)
        {
            throw new InvalidOperationException($"User with username {userDto.Username} not found.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, userDto.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Error resetting password.");
        }
    }

    public async Task LogoutUserAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public AuthTokenDto GenerateJwtToken(string userId)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
        };

        var tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value ?? "";
        SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(tokenKeyString));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor descriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddMinutes(30)
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken token = tokenHandler.CreateToken(descriptor);

        return new AuthTokenDto
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        };
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new InvalidOperationException($"User with email {email} not found.");
        }
        return user;
    }


    public async Task<string> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        return user.UserName!;
    }

    public class LogInResult
    {
        public bool Succeeded { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public string FailureReason { get; set; } = ""; // Add a property for detailed failure reasons
    }

}