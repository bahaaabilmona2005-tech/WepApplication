using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication1.DTOs.User;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize] // All endpoints require authentication by default
public class UsersController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IDSDatabaseDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ===================== GET ALL USERS (Admin only) =====================
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _context.Users
                .Where(u => u.Status == "Active") // Only active users
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    Status = u.Status,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, "An error occurred while fetching users");
        }
    }

    // ===================== GET USER BY ID (Admin or self) =====================
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            // Get current user's ID from claims
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // Check if user is Admin or requesting their own data
            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid(); // User can only access their own data
            }

            var user = await _context.Users
                .Where(u => u.Id == id && u.Status == "Active")
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    Status = u.Status,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { Message = "User not found or inactive" });

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while fetching user");
        }
    }

    // ===================== CREATE USER (Admin only) =====================
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return BadRequest(new { Message = "Email already exists" });

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role ?? "User", // Default to "User" if not specified
                Status = dto.Status ?? "Active", // Default to "Active" if not specified
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return user without sensitive data
            return CreatedAtAction(nameof(Get), new { id = user.Id }, new
            {
                Message = "User created successfully",
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                Status = user.Status
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with email: {Email}", dto.Email);
            return StatusCode(500, "An error occurred while creating user");
        }
    }

    // ===================== UPDATE USER (Admin only) =====================
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            // Check if email is being changed and already exists for another user
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email.ToLower() != user.Email.ToLower())
            {
                if (await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.Id != id))
                    return BadRequest(new { Message = "Email already exists" });
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(dto.FullName))
                user.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Role))
                user.Role = dto.Role;

            
            // Only update password if provided
            if (!string.IsNullOrEmpty(dto.Password))
                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "User updated successfully",
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                Status = user.Status
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while updating user");
        }
    }

    // ===================== DELETE USER (Admin only) =====================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            // Soft delete: change status to Inactive instead of removing
            user.Status = "Inactive";
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "User deactivated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while deleting user");
        }
    }

    // ===================== UPDATE USER PROFILE (User can update their own profile) =====================
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UserUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get current user's ID from claims
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.Users.FindAsync(currentUserId);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            // Users can only update their name and password (not email or role)
            if (!string.IsNullOrEmpty(dto.FullName))
                user.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Password))
                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Profile updated successfully",
                FullName = user.FullName,
                Email = user.Email
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile for user");
            return StatusCode(500, "An error occurred while updating profile");
        }
    }

    // ===================== SEARCH USERS (Admin only) =====================
    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { Message = "Search query is required" });

            var users = await _context.Users
                .Where(u => u.Status == "Active" &&
                           (u.FullName.Contains(query) ||
                            u.Email.Contains(query) ||
                            u.Role.Contains(query)))
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    Status = u.Status,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with query: {Query}", query);
            return StatusCode(500, "An error occurred while searching users");
        }
    }
}


   

  
