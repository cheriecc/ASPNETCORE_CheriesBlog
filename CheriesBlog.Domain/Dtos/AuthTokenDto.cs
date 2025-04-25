using System;

namespace CheriesBlog.Domain.Dtos;

public class AuthTokenDto
{
    public string Token { get; set; } = "";
    public DateTime Expiration { get; set; }

}
