using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

[Index("Email", Name = "UQ__Users__A9D10534B26D7807", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string HashedPassword { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Role { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("User")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("User")]
    public virtual ICollection<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();

    [InverseProperty("User")]
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
