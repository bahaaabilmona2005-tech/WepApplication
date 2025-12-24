using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("api/[controller]")]
public class QuizAttemptsController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public QuizAttemptsController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(QuizAttempt attempt)
    {
        _context.QuizAttempts.Add(attempt);
        await _context.SaveChangesAsync();
        return Ok(attempt);
    }
}
