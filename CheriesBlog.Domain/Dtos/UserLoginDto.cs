using System;

namespace CheriesBlog.Domain.Dtos;

public class UserLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
