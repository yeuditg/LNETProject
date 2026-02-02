using WebApplication1.Core.Interfaces;

namespace WebApplication1.Services
{
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public IQueryable<T> GetAll() => _repository.GetAll();

        public T? GetById(int id) => _repository.GetById(id);

        public void Add(T entity)
        {
            _repository.Add(entity);
            _repository.Save();
        }
    }
}
