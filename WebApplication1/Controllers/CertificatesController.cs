using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public CertificatesController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Certificate certificate)
    {
        _context.Certificates.Add(certificate);
        await _context.SaveChangesAsync();
        return Ok(certificate);
    }
}
