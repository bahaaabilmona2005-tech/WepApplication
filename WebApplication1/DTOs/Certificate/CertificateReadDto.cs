namespace WebApplication1.DTOs.Certificate
{
    public class CertificateReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
