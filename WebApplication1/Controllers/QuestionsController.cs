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
public class QuestionsController : ControllerBase
{
    private readonly IDSDatabaseDbContext _context;

    public QuestionsController(IDSDatabaseDbContext context)
    {
        _context = context;
    }

    [HttpGet("quiz/{quizId}")]
    public async Task<IActionResult> GetByQuiz(int quizId)
        => Ok(await _context.Questions.Where(q => q.QuizId == quizId).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return Ok(question);
    }
}
