using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

public partial class Question
{
    [Key]
    public int Id { get; set; }

    public int QuizId { get; set; }

    [Column(TypeName = "text")]
    public string QuestionText { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string QuestionType { get; set; } = null!;

    public int? Points { get; set; }

    public int OrderIndex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [ForeignKey("QuizId")]
    [InverseProperty("Questions")]
    public virtual Quiz Quiz { get; set; } = null!;

    [InverseProperty("Question")]
    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();
}
