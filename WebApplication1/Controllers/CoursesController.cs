using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.DTOs.Course;

[ApiController]
[Route("api/[controller]")]
[Authorize] // All endpoints require authentication
public class CoursesController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(IDSDatabaseDbContext context, ILogger<CoursesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ===================== GET ALL COURSES (Admin, Instructor, User) =====================
    [HttpGet]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var courses = await _context.Courses
                
                .Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.LongDescription,
                    Category = c.Category,
                    Difficulty = c.Difficulty,
                    CreatedAt = c.CreatedAt ?? DateTime.MinValue,
                    UpdatedAt = c.UpdatedAt ?? DateTime.MinValue
                })
                .ToListAsync();

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all courses");
            return StatusCode(500, "An error occurred while fetching courses");
        }
    }

    // ===================== GET COURSE BY ID (Admin, Instructor, User) =====================
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var course = await _context.Courses
                
                .Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.LongDescription,
                    Category = c.Category,
                    Difficulty = c.Difficulty,
                    CreatedAt = c.CreatedAt ?? DateTime.MinValue,
                    UpdatedAt = c.UpdatedAt ?? DateTime.MinValue
                })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound(new { Message = "Course not found or inactive" });

            return Ok(course);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting course with ID: {CourseId}", id);
            return StatusCode(500, "An error occurred while fetching course");
        }
    }

    // ===================== CREATE COURSE (Admin, Instructor) =====================
    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Create(CourseCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if course title already exists (case-insensitive)
            if (await _context.Courses.AnyAsync(c =>
                c.Title.ToLower() == dto.Title.ToLower()))
                return BadRequest(new { Message = "Course title already exists" });

            var course = new Course
            {
                Title = dto.Title,
                LongDescription = dto.Description,
                Category = dto.Category,
                Difficulty = dto.Difficulty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = course.Id }, new
            {
                Message = "Course created successfully",
                Id = course.Id,
                Title = course.Title,
                Description = course.LongDescription,
                Category = course.Category,
                Difficulty = course.Difficulty,
        
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course with title: {Title}", dto.Title);
            return StatusCode(500, "An error occurred while creating course");
        }
    }

    // ===================== UPDATE COURSE (Admin, Instructor) =====================
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Update(int id, CourseUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { Message = "Course not found" });

            // Check if title is being changed and already exists for another active course
            if (!string.IsNullOrEmpty(dto.Title) && dto.Title.ToLower() != course.Title.ToLower())
            {
                if (await _context.Courses.AnyAsync(c =>
                    c.Title.ToLower() == dto.Title.ToLower() &&
                    c.Id != id ))
                    return BadRequest(new { Message = "Course title already exists" });
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(dto.Title))
                course.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Description))
                course.LongDescription = dto.Description;

            if (!string.IsNullOrEmpty(dto.Category))
                course.Category = dto.Category;

            if (!string.IsNullOrEmpty(dto.Difficulty))
                course.Difficulty = dto.Difficulty;

            

            course.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Course updated successfully",
                Id = course.Id,
                Title = course.Title,
                Description = course.LongDescription,
                Category = course.Category,
                Difficulty = course.Difficulty,
                
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course with ID: {CourseId}", id);
            return StatusCode(500, "An error occurred while updating course");
        }
    }

    // ===================== DELETE COURSE (Admin only - Soft Delete) =====================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { Message = "Course not found" });

            // Soft delete: change status to Inactive instead of removing
           
            course.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Course deactivated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course with ID: {CourseId}", id);
            return StatusCode(500, "An error occurred while deleting course");
        }
    }

    // ===================== SEARCH COURSES (Admin, Instructor, Student) =====================
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { Message = "Search query is required" });

            var courses = await _context.Courses
                .Where(c => (c.Title.Contains(query) ||
                            c.LongDescription.Contains(query) ||
                            c.Category.Contains(query)))
                .Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.LongDescription,
                    Category = c.Category,
                    Difficulty = c.Difficulty,
                    
                    CreatedAt = c.CreatedAt ?? DateTime.MinValue,
                    UpdatedAt = c.UpdatedAt ?? DateTime.MinValue
                })
                .ToListAsync();

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching courses with query: {Query}", query);
            return StatusCode(500, "An error occurred while searching courses");
        }
    }

    // ===================== GET COURSES BY CATEGORY =====================
    [HttpGet("category/{category}")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        try
        {
            var courses = await _context.Courses
                .Where(c => c.Category.ToLower() == category.ToLower())
                .Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.LongDescription,
                    Category = c.Category,
                    Difficulty = c.Difficulty,
                    CreatedAt = c.CreatedAt ?? DateTime.MinValue,
                    UpdatedAt = c.UpdatedAt ?? DateTime.MinValue
                })
                .ToListAsync();

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting courses by category: {Category}", category);
            return StatusCode(500, "An error occurred while fetching courses by category");
        }
    }

    // ===================== GET COURSES BY DIFFICULTY =====================
    [HttpGet("difficulty/{difficulty}")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> GetByDifficulty(string difficulty)
    {
        try
        {
            var courses = await _context.Courses
                .Where(c => c.Difficulty.ToLower() == difficulty.ToLower())
                .Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.LongDescription,
                    Category = c.Category,
                    Difficulty = c.Difficulty,
                    CreatedAt = c.CreatedAt ?? DateTime.MinValue,
                    UpdatedAt = c.UpdatedAt ?? DateTime.MinValue
                })
                .ToListAsync();

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting courses by difficulty: {Difficulty}", difficulty);
            return StatusCode(500, "An error occurred while fetching courses by difficulty");
        }
    }
}


