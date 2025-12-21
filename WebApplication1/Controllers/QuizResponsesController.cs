using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Student")]
public class QuizResponsesController : ControllerBase
{
    [HttpPost]
    public IActionResult Submit(QuizResponse response)
    {
        using var context = new IDSDatabaseDbContext();
        context.QuizResponses.Add(response);
        context.SaveChanges();
        return Ok(response);
    }
}
