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


[Authorize(Roles = "Admin,User")]
[ApiController]
[Route("api/[controller]")]
public class QuizAttemptsController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    public QuizAttemptsController(IDSDatabaseDbContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> Create(QuizAttempt attempt)
    {
        _context.QuizAttempts.Add(attempt);
        await _context.SaveChangesAsync();
        return Ok(attempt);
    }
}
