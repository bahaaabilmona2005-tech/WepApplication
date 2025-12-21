using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        using var context = new IDSDatabaseDbContext();
        return Ok(context.Courses.Where(c => c.IsPublished == true).ToList());
    }

    [Authorize(Roles = "Instructor,Admin")]
    [HttpPost]
    public IActionResult Create(Course course)
    {
        using var context = new IDSDatabaseDbContext();

        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        course.CreatedBy = userId;
        course.CreatedAt = DateTime.UtcNow;

        context.Courses.Add(course);
        context.SaveChanges();
        return Ok(course);
    }

    [Authorize(Roles = "Instructor,Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        using var context = new IDSDatabaseDbContext();
        var course = context.Courses.Find(id);
        if (course == null) return NotFound();

        context.Courses.Remove(course);
        context.SaveChanges();
        return Ok();
    }
}
