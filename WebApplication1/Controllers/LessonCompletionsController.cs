using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("api/[controller]")]
public class LessonCompletionsController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public LessonCompletionsController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Complete(LessonCompletion completion)
    {
        _context.LessonCompletions.Add(completion);
        await _context.SaveChangesAsync();
        return Ok(completion);
    }
}
