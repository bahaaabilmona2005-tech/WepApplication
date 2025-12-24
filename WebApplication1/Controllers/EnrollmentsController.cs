using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public EnrollmentsController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Enroll(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
        return Ok(enrollment);
    }
}
