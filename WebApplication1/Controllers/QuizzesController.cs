using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize]
public class QuizzesController : ControllerBase
{
    [Authorize(Roles = "Instructor,Admin")]
    [HttpPost]
    public IActionResult Create(Quiz quiz)
    {
        using var context = new IDSDatabaseDbContext();
        context.Quizzes.Add(quiz);
        context.SaveChanges();
        return Ok(quiz);
    }

    [HttpGet("{courseId}")]
    public IActionResult GetByCourse(int courseId)
    {
        using var context = new IDSDatabaseDbContext();
        return Ok(context.Quizzes.Where(q => q.CourseId == courseId).ToList());
    }
}
