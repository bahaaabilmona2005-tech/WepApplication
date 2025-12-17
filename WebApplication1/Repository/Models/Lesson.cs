using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

public partial class Lesson
{
    [Key]
    public int Id { get; set; }

    public int CourseId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Content { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? VideoUrl { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AttachmentUrl { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string LessonType { get; set; } = null!;

    public int OrderIndex { get; set; }

    public int? EstimatedDuration { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Lessons")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Lesson")]
    public virtual ICollection<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();

    [InverseProperty("Lesson")]
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
