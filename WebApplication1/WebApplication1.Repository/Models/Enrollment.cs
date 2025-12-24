using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

[Index("UserId", "CourseId", Name = "UQ_Enrollment", IsUnique = true)]
public partial class Enrollment
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EnrolledAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CompletedAt { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? ProgressPercentage { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Enrollments")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Enrollments")]
    public virtual User User { get; set; } = null!;
}
