using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Student")]
public class EnrollmentsController : ControllerBase
{
    [HttpPost]
    public IActionResult Enroll(Enrollment enrollment)
    {
        using var context = new IDSDatabaseDbContext();

        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        enrollment.UserId = userId;
        enrollment.EnrolledAt = DateTime.UtcNow;

        context.Enrollments.Add(enrollment);
        context.SaveChanges();
        return Ok(enrollment);
    }
}
