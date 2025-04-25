using System;

namespace CheriesBlog.Domain.Dtos;

public class UserRegistrationDto
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string PasswordConfirmation { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}
