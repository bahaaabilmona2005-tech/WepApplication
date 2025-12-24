using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository.Models;

[Index("CourseId", "UserId", Name = "UQ_Certificates", IsUnique = true)]
[Index("VerificationCode", Name = "UQ__Certific__DA24CB14FDE48CF6", IsUnique = true)]
public partial class Certificate
{
    [Key]
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int UserId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string VerificationCode { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? DownloadUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? GeneratedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Certificates")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Certificates")]
    public virtual User User { get; set; } = null!;
}
