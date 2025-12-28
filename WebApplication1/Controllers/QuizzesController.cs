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
public class QuizzesController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    public QuizzesController(IDSDatabaseDbContext context) => _context = context;

    [Authorize(Roles = "Admin,Instructor,User")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _context.Quizzes.ToListAsync());

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPost]
    public async Task<IActionResult> Create(Quiz quiz)
    {
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return Ok(quiz);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Quiz quiz)
    {
        if (id != quiz.Id) return BadRequest();
        _context.Entry(quiz).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var quiz = await _context.Quizzes.FindAsync(id);
        if (quiz == null) return NotFound();
        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
