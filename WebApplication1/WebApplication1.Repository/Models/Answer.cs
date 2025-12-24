using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

public partial class Answer
{
    [Key]
    public int Id { get; set; }

    public int QuestionId { get; set; }

    [Column(TypeName = "text")]
    public string AnswerText { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public int OrderIndex { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("Answers")]
    public virtual Question Question { get; set; } = null!;

    [InverseProperty("SelectedAnswer")]
    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();
}
