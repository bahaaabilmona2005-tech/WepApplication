namespace WebApplication1.DTOs.LessonCompletion
{
    public class LessonCompletionReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
