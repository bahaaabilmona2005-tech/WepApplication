using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs.Auth;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(IDSDatabaseDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // ===================== REGISTER =====================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // 1️⃣ Check email uniqueness
        bool emailExists = await _context.Users
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExists)
            return BadRequest("Email already exists");

        // 2️⃣ Create user
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User",          // Default role
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // 3️⃣ Save to DB
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    // ===================== LOGIN =====================
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        // 1️⃣ Find active user
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == dto.Email &&
                u.Status == "Active");

        if (user == null)
            return Unauthorized("Invalid email or password");

        // 2️⃣ Verify password
        bool validPassword = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.HashedPassword);

        if (!validPassword)
            return Unauthorized("Invalid email or password");

        // 3️⃣ Create claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // 4️⃣ Create JWT token
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        // 5️⃣ Return token
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            role = user.Role
        });
    }
}