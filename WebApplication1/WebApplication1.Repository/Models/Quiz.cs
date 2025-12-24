using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

public partial class Quiz
{
    [Key]
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int? LessonId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    public int PassingScore { get; set; }

    public int? TimeLimit { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Quizzes")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("LessonId")]
    [InverseProperty("Quizzes")]
    public virtual Lesson? Lesson { get; set; }

    [InverseProperty("Quiz")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [InverseProperty("Quiz")]
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
