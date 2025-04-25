using System;

namespace CheriesBlog.Domain.Dtos;

public class UserResetPasswordDto
{

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string PasswordConfirmation { get; set; } = "";
}
