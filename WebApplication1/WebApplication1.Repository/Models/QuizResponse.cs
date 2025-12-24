using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

public partial class QuizResponse
{
    [Key]
    public int Id { get; set; }

    public int AttemptId { get; set; }

    public int QuestionId { get; set; }

    public int? SelectedAnswerId { get; set; }

    [Column(TypeName = "text")]
    public string? TextAnswer { get; set; }

    public bool IsCorrect { get; set; }

    [ForeignKey("AttemptId")]
    [InverseProperty("QuizResponses")]
    public virtual QuizAttempt Attempt { get; set; } = null!;

    [ForeignKey("QuestionId")]
    [InverseProperty("QuizResponses")]
    public virtual Question Question { get; set; } = null!;

    [ForeignKey("SelectedAnswerId")]
    [InverseProperty("QuizResponses")]
    public virtual Answer? SelectedAnswer { get; set; }
}
