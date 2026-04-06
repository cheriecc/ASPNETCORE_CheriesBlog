using System;

namespace CheriesBlog.Domain.Dtos;

public class UserRegistrationDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordConfirmation { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
