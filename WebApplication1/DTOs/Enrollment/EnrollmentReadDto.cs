namespace WebApplication1.DTOs.Enrollment
{
    public class EnrollmentReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
