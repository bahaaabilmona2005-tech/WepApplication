using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize]
public class LessonsController : ControllerBase
{
    [HttpGet("course/{courseId}")]
    public IActionResult GetLessons(int courseId)
    {
        using var context = new IDSDatabaseDbContext();
        return Ok(context.Lessons
            .Where(l => l.CourseId == courseId)
            .OrderBy(l => l.OrderIndex)
            .ToList());
    }

    [Authorize(Roles = "Instructor,Admin")]
    [HttpPost]
    public IActionResult Create(Lesson lesson)
    {
        using var context = new IDSDatabaseDbContext();
        lesson.CreatedAt = DateTime.UtcNow;
        context.Lessons.Add(lesson);
        context.SaveChanges();
        return Ok(lesson);
    }
}
