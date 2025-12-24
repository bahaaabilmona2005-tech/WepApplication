namespace WebApplication1.DTOs.QuizResponse
{
    public class QuizResponseReadDto
    {
        public int Id { get; set; }
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
