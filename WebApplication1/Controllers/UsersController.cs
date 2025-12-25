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

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public UsersController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _context.Users.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest ("email already exists");
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };
         _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.Id) return BadRequest();
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
