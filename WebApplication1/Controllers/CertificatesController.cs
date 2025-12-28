using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using WebApplication1.DTOs.User;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    public CertificatesController(IDSDatabaseDbContext context) => _context = context;

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _context.Certificates.ToListAsync());
}
