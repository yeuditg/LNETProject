namespace WebApplication1.Core.DTOs
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Votes { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
