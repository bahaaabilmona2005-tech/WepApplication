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


[Authorize(Roles = "Admin,Instructor")]
[ApiController]
[Route("api/[controller]")]
public class AnswersController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    public AnswersController(IDSDatabaseDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _context.Answers.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Answer answer)
    {
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
        return Ok(answer);
    }
}
