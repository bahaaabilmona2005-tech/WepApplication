using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Instructor,Admin")]
public class AnswersController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(Answer answer)
    {
        using var context = new IDSDatabaseDbContext();
        context.Answers.Add(answer);
        context.SaveChanges();
        return Ok(answer);
    }
}
