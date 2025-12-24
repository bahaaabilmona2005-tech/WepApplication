using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

public partial class Course
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string ShortDescription { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Category { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? LongDescription { get; set; }

    public int? EstimatedDuration { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Thumbnail { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public bool? IsPublished { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Difficulty { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("Courses")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Course")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [InverseProperty("Course")]
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
