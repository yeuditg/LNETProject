using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Interfaces;
using WebApplication1.Core.Models;

namespace WebApplication1.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<Answer> _answerRepo;

        public QuestionService(
            IRepository<Question> questionRepo,
            IRepository<Answer> answerRepo)
        {
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
        }

        public CreateQuestionResponse Create(CreateQuestionRequest request)
        {
            if (request.Answers.Count < 2)
                throw new Exception("נדרשות לפחות שתי תשובות");

            if (request.Type == QuestionType.Trivia &&
                request.Answers.Count(a => a.IsCorrect) != 1)
                throw new Exception("לטריוויה חייבת להיות תשובה נכונה אחת");

            var question = new Question
            {
                Text = request.Text,
                Type = request.Type,
                Answers = request.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            _questionRepo.Add(question);
            _questionRepo.Save();

            return new CreateQuestionResponse
            {
                QuestionId = question.Id
            };
        }

        public GetQuestionResponse Get(int id)
        {
            var question = _questionRepo.GetAll()
                .Include(q => q.Answers)
                .FirstOrDefault(q => q.Id == id);

            if (question == null)
                throw new Exception("שאלה לא נמצאה");

            return new GetQuestionResponse
            {
                Id = question.Id,
                Text = question.Text,
                Type = question.Type,
                Answers = question.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    Votes = a.Votes,
                    IsCorrect= a.IsCorrect
                }).ToList()
            };
        }
        public List<GetQuestionResponse> GetAll()
        {
            var questions = _questionRepo.GetAll().ToList();

            if (!questions.Any())
                throw new Exception("אין שאלות");

            return questions.Select(q => new GetQuestionResponse
            {
                Id = q.Id,
                Text = q.Text,
                Type = q.Type,
                Answers = q.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    Votes = a.Votes
                }).ToList()

            }).ToList();
        }

        public VoteResponse Vote(VoteRequest request)
        {
            var answer = _answerRepo.GetAll()
                .Include(a => a.Question)
                .FirstOrDefault(a =>
                    a.Id == request.AnswerId &&
                    a.QuestionId == request.QuestionId);

            if (answer == null)
                throw new Exception("תשובה לא קיימת");

            answer.Votes++;
            _answerRepo.Save();

            return new VoteResponse
            {
                Votes = answer.Votes,
                IsCorrect = answer.Question.Type == QuestionType.Trivia
                    ? answer.IsCorrect
                    : null
            };
        }

    }
}