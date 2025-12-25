using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.DTOs.Auth
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }

}

