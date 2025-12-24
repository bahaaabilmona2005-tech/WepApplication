namespace WebApplication1.DTOs.Answer
{
    public class AnswerCreateDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
