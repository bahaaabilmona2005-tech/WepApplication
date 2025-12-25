using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.DTOs.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
