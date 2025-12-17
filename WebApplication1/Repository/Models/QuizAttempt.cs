using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

public partial class QuizAttempt
{
    [Key]
    public int Id { get; set; }

    public int QuizId { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Score { get; set; }

    public int TotalPoints { get; set; }

    public int EarnedPoints { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SubmittedAt { get; set; }

    public int? TimeTaken { get; set; }

    [ForeignKey("QuizId")]
    [InverseProperty("QuizAttempts")]
    public virtual Quiz Quiz { get; set; } = null!;

    [InverseProperty("Attempt")]
    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();

    [ForeignKey("UserId")]
    [InverseProperty("QuizAttempts")]
    public virtual User User { get; set; } = null!;
}
