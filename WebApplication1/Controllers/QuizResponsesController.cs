using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("api/[controller]")]
public class QuizResponsesController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public QuizResponsesController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(QuizResponse response)
    {
        _context.QuizResponses.Add(response);
        await _context.SaveChangesAsync();
        return Ok(response);
    }
}
