namespace WebApplication1.Core.Models
{
    public enum QuestionType
    {
        Trivia = 1,
        Poll = 2
    }
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }

}
