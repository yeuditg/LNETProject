namespace WebApplication1.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
        void Save();
    }
}
