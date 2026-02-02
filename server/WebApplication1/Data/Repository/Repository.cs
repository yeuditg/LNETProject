using WebApplication1.Core.Interfaces;
using WebApplication1.Data;

namespace WebApplication1.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }
        virtual
        public IQueryable<T> GetAll() => _context.Set<T>();

        public T? GetById(int id) => _context.Set<T>().Find(id);

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
