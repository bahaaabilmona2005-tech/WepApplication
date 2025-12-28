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
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IDSDatabaseDbContext context,
        IConfiguration config,
        ILogger<AuthController> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    // ===================== REGISTER =====================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            // 1️⃣ Validate input
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.FullName))
                return BadRequest("Email and full name are required");

            // 2️⃣ Check email uniqueness
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExists)
                return BadRequest("Email already exists");

            // 3️⃣ Create user (no password needed for your use case)
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                HashedPassword = null, // Not needed for your authentication method
                Role = "User",          // Default role
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // 4️⃣ Save to DB
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 5️⃣ Return success without sensitive data
            return Ok(new
            {
                Message = "User registered successfully",
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", dto.Email);
            return StatusCode(500, "An error occurred during registration");
        }
    }

    // ===================== LOGIN (Email + FullName/Username) =====================
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            // 1️⃣ Validate input
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Username))
                return BadRequest("Email and username are required");

            // 2️⃣ Find active user with matching email AND full name (username)
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email.ToLower() == dto.Email.ToLower() &&
                    u.FullName.ToLower() == dto.Username.ToLower() &&
                    u.Status == "Active");

            if (user == null)
            {
                _logger.LogWarning("Login failed: No active user found with email: {Email} and username: {Username}",
                    dto.Email, dto.Username);
                return Unauthorized(new { Message = "Invalid email or username" });
            }

            // 3️⃣ Get JWT configuration
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                _logger.LogError("JWT Key is not configured");
                return StatusCode(500, "Server configuration error");
            }

            // 4️⃣ Create claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // 5️⃣ Create JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Get token expiration from config or default to 3 hours
            var expireMinutes = _config.GetValue<int>("Jwt:ExpireMinutes", 180);
            var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 6️⃣ Return token (you can paste this in Authorization header)
            return Ok(new
            {
                Token = tokenString,
                TokenType = "Bearer",
                Expires = expires,
                User = new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                },
                Instructions = "Copy the token and add to Authorization header as: 'Bearer YOUR_TOKEN_HERE'"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email} and username: {Username}",
                dto.Email, dto.Username);
            return StatusCode(500, "An error occurred during login");
        }
    }

    // ===================== GET CURRENT USER INFO =====================
    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize] // Requires authentication
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            // Get user ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user ID");

            // Find user
            var user = await _context.Users
                .Where(u => u.Id == userId && u.Status == "Active")
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FullName,
                    u.Role,
                    u.Status,
                    u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user info");
            return StatusCode(500, "An error occurred");
        }
    }
}
