namespace WebApplication1.DTOs.QuizAttempt
{
    public class QuizAttemptReadDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
    }
}
