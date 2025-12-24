using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AnswersController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public AnswersController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpGet("question/{questionId}")]
    public async Task<IActionResult> GetByQuestion(int questionId)
        => Ok(await _context.Answers.Where(a => a.QuestionId == questionId).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Answer answer)
    {
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
        return Ok(answer);
    }
}
