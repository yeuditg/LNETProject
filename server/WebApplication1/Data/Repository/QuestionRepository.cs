using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Interfaces;
using WebApplication1.Core.Models;

namespace WebApplication1.Data.Repository
{
    public class QuestionRepository : Repository<Question>, IRepository<Question>
    {
        public QuestionRepository(DataContext context) : base(context)
        {
        }
        override
        public IQueryable<Question> GetAll()
        {
            return _context.Questions.Include(q => q.Answers);
        }
    }
}
