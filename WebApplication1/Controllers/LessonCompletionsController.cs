using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Student")]
public class LessonCompletionsController : ControllerBase
{
    [HttpPost]
    public IActionResult Complete(LessonCompletion completion)
    {
        using var context = new IDSDatabaseDbContext();

        completion.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        completion.CompletedAt = DateTime.UtcNow;

        context.LessonCompletions.Add(completion);
        context.SaveChanges();
        return Ok(completion);
    }
}
