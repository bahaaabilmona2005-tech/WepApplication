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
//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;
    public LessonsController(IDSDatabaseDbContext context) => _context = context;

    //[Authorize(Roles = "Admin,Instructor,User")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _context.Lessons.ToListAsync());

    //[Authorize(Roles = "Admin,Instructor")]
    [HttpPost]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return Ok(lesson);
    }

    //[Authorize(Roles = "Admin,Instructor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Lesson lesson)
    {
        if (id != lesson.Id) return BadRequest();
        _context.Entry(lesson).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null) return NotFound();
        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
