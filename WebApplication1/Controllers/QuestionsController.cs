using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Instructor,Admin")]
public class QuestionsController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(Question question)
    {
        using var context = new IDSDatabaseDbContext();
        context.Questions.Add(question);
        context.SaveChanges();
        return Ok(question);
    }
}
