

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication1.Core.Models;
//using WebApplication1.Data;

//namespace WebApplication1.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QuestionController : ControllerBase
//    {
//        private readonly DataContext _context;

//        public QuestionController(DataContext context) => _context = context;

//        // GET: api/question
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Question>>> GetAll()
//        {
//            var questions = await _context.Questions
//                .Include(q => q.Answers)
//                .ToListAsync();

//            return Ok(questions);
//        }

//        // GET: api/question/{id}
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Question>> GetById(int id)
//        {
//            var question = await _context.Questions
//                .Include(q => q.Answers)
//                .FirstOrDefaultAsync(q => q.Id == id);

//            if (question == null)
//                return NotFound("השאלה לא נמצאה");

//            return Ok(question);
//        }

//        // POST: api/question/create
//        [HttpPost("create")]
//        public async Task<ActionResult<int>> CreateQuestion([FromBody] Question question)
//        {
//            // Validation
//            var validationError = ValidateQuestion(question);
//            if (validationError != null)
//                return BadRequest(validationError);

//            _context.Questions.Add(question);

//            await _context.SaveChangesAsync();
//            // i have the id after savechanges
//            for (int i = 0; i < question.Answers.Count; i++)
//            {
//                question.Answers[i].QuestionId = question.Id;
//                _context.Answers.Add(question.Answers[i]);
//            }
//            _context.SaveChanges();

//            return Ok(question.Id);
//        }
//        [HttpPost("vote/{questionId}/answer/{voteAnswerId}")]
//        public async Task<ActionResult<object>> Vote(int questionId,int voteAnswerId)
//        {
//            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == voteAnswerId && a.QuestionId == questionId);

//            if (answer == null)
//                return NotFound("התשובה לא נמצאה");

//            // Increment vote
//            answer.Votes++;
//            await _context.SaveChangesAsync();
//            Question question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
//            if (question.Type == QuestionType.Trivia)
//            {
//                return Ok(new { answer.Votes, answer.IsCorrect });
//            }
//            return Ok(new { answer.Votes });
//        }

//        //// PUT: api/question/{id}
//        //[HttpPut("{id}")]
//        //public async Task<ActionResult> UpdateQuestion(int id, [FromBody] Question updatedQuestion)
//        //{
//        //    var question = await _context.Questions
//        //        .Include(q => q.Answers)
//        //        .FirstOrDefaultAsync(q => q.Id == id);

//        //    if (question == null)
//        //        return NotFound("השאלה לא נמצאה");

//        //    // Validation
//        //    var validationError = ValidateQuestion(updatedQuestion);
//        //    if (validationError != null)
//        //        return BadRequest(validationError);

//        //    // Update question properties
//        //    question.Text = updatedQuestion.Text;
//        //    question.Type = updatedQuestion.Type;

//        //    // Update answers efficiently
//        //    if (updatedQuestion.Answers != null)
//        //    {
//        //        // Remove answers that are no longer in the updated list
//        //        var updatedAnswerIds = updatedQuestion.Answers
//        //            .Where(a => a.Id > 0)
//        //            .Select(a => a.Id)
//        //            .ToList();

//        //        var answersToRemove = question.Answers
//        //            .Where(a => !updatedAnswerIds.Contains(a.Id))
//        //            .ToList();

//        //        _context.Answers.RemoveRange(answersToRemove);

//        //        // Update existing answers and add new ones
//        //        foreach (var updatedAnswer in updatedQuestion.Answers)
//        //        {
//        //            if (updatedAnswer.Id > 0)
//        //            {
//        //                // Update existing answer
//        //                var existingAnswer = question.Answers
//        //                    .FirstOrDefault(a => a.Id == updatedAnswer.Id);

//        //                if (existingAnswer != null)
//        //                {
//        //                    existingAnswer.Text = updatedAnswer.Text;
//        //                    existingAnswer.IsCorrect = updatedAnswer.IsCorrect;
//        //                }
//        //            }
//        //            else
//        //            {
//        //                // Add new answer
//        //                var newAnswer = new Answer
//        //                {
//        //                    Text = updatedAnswer.Text,
//        //                    IsCorrect = updatedAnswer.IsCorrect,
//        //                    QuestionId = question.Id
//        //                };
//        //                question.Answers.Add(newAnswer);
//        //            }
//        //        }
//        //    }

//        //    await _context.SaveChangesAsync();
//        //    return Ok("השאלה עודכנה בהצלחה");
//        //}

//        //// DELETE: api/question/{id}
//        //[HttpDelete("{id}")]
//        //public async Task<ActionResult> DeleteQuestion(int id)
//        //{
//        //    var question = await _context.Questions
//        //        .Include(q => q.Answers)
//        //        .FirstOrDefaultAsync(q => q.Id == id);

//        //    if (question == null)
//        //        return NotFound("השאלה לא נמצאה");

//        //    _context.Questions.Remove(question);
//        //    await _context.SaveChangesAsync();

//        //    return Ok("השאלה נמחקה בהצלחה");
//        //}

//        // Validation helper method
//        private string ValidateQuestion(Question question)
//        {
//            if (string.IsNullOrWhiteSpace(question.Text))
//                return "השאלה חייבת להכיל טקסט";

//            if (!Enum.IsDefined(typeof(QuestionType), question.Type))
//                return "סוג השאלה לא תקין";

//            // ✅ אם אין תשובות - זה בסדר! (נוסיף אותן אחר כך)
//            if (question.Answers == null || !question.Answers.Any())
//                return null; // שאלה בלי תשובות - מותר!

//            // Validate each answer has text
//            if (question.Answers.Any(a => string.IsNullOrWhiteSpace(a.Text)))
//                return "כל תשובה חייבת להכיל טקסט";

//            // Type-specific validation - רק אם יש תשובות!
//            if (question.Type == QuestionType.Trivia)
//            {
//                var correctAnswersCount = question.Answers.Count(a => a.IsCorrect);
//                if (correctAnswersCount > 1)
//                    return "שאלות Trivia חייבות להכיל לא יותר מתשובה נכונה אחת";
//            }

//            return null;
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<CreateQuestionResponse> Create(CreateQuestionRequest request)
        {
            try
            {
                var result = _service.Create(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // אפשר לשנות ל-StatusCode לפי סוג השגיאה
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<GetQuestionResponse> Get(int id)
        {
            try
            {
                var result = _service.Get(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult<GetQuestionResponse> GetAll()
        {
            try
            {
                var result = _service.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("vote")]
        public ActionResult<VoteResponse> Vote(VoteRequest request)
        {
            try
            {
                var result = _service.Vote(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

