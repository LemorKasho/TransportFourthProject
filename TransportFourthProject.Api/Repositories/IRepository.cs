using System.Linq.Expressions;

namespace TransportFourthProject.Api.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        void Remove(T entity);
        Task SaveChangesAsync();
    }
}
