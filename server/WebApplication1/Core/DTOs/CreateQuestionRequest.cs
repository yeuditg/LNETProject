using WebApplication1.Core.Models;

namespace WebApplication1.Core.DTOs
{
    public class CreateQuestionRequest
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<CreateAnswerDto> Answers { get; set; }
    }
}
