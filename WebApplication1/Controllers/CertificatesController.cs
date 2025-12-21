using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Student")]
public class CertificatesController : ControllerBase
{
    [HttpGet]
    public IActionResult MyCertificates()
    {
        using var context = new IDSDatabaseDbContext();

        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        return Ok(context.Certificates.Where(c => c.UserId == userId).ToList());
    }
}
