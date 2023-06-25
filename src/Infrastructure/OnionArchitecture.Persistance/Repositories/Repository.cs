using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Common;
using OnionArchitecture.Persistance.Contexts;
using System.Linq.Expressions;

namespace OnionArchitecture.Persistance.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly TodoDbContext _context;

        public Repository(TodoDbContext todoDbContext)
        {
            _context = todoDbContext;
        }

        private DbSet<T> Table => _context.Set<T>();
        public async Task<bool> AddAsync(T entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.CreatedDate = DateTime.Now;
            entity.IsDeleted = false;
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }
        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            entities.ForEach(x =>
            {
                x.ModifiedDate = DateTime.Now;
                x.CreatedDate = DateTime.Now;
                x.IsDeleted = false;
            });
            await Table.AddRangeAsync(entities);
            return true;
        }
        public bool Delete(T entity)
        {
            EntityEntry<T> entityEntry = Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }
        public bool PassiveDelete(T entity)
        {
            entity.IsDeleted = true;
            entity.ModifiedDate = DateTime.Now;
            return true;
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (filter != null)
            {
                query = Table.Where(filter);
            }
            if (!tracking)
                query.AsNoTracking();
            return query;
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();

            var response = await query.FirstOrDefaultAsync(filter);
            return response;
        }
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public bool Update(T entity)
        {
            entity.ModifiedDate = DateTime.Now;
            EntityEntry entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
        public bool UpdateRange(List<T> entites)
        {
            entites.ForEach(x => x.ModifiedDate = DateTime.Now);
            Table.UpdateRange(entites);
            return true;
        }

        public bool DeleteRange(List<T> entites)
        {
            Table.RemoveRange(entites);
            return true;

        }
        public bool PassiveDeleteRange(List<T> entites)
        {
            foreach (var entity in entites)
            {
                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.Now;
            }
            return UpdateRange(entites);

        }
    }
}
