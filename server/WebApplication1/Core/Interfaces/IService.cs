namespace WebApplication1.Core.Interfaces
{
    public interface IService<T> where T : class
    {
        IQueryable<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
    }
}
