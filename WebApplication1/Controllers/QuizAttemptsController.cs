using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Student")]
public class QuizAttemptsController : ControllerBase
{
    [HttpPost]
    public IActionResult Start(QuizAttempt attempt)
    {
        using var context = new IDSDatabaseDbContext();

        attempt.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        attempt.StartedAt = DateTime.UtcNow;

        context.QuizAttempts.Add(attempt);
        context.SaveChanges();
        return Ok(attempt);
    }
}
