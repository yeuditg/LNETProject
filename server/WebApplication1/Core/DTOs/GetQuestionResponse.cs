using WebApplication1.Core.Models;

namespace WebApplication1.Core.DTOs
{
    public class GetQuestionResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}
