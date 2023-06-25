using OnionArchitecture.Domain.Common;
using System.Linq.Expressions;

namespace OnionArchitecture.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        Task<bool> AddAsync(T entity);
        Task<int> SaveAsync();
        Task<bool> AddRangeAsync(List<T> entities);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true);
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, bool tracking = true);
        bool Update(T entity);
        bool UpdateRange(List<T> entites);
        bool PassiveDelete(T entity);
        bool Delete(T entity);
        bool DeleteRange(List<T> entites);
        bool PassiveDeleteRange(List<T> entites);
    }
}
