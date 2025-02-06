using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Entities;

namespace TaskTracker.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public RepositoryBase(TodoContext todoContext)
        {
            TodoContext = todoContext;
        }
        
        protected TodoContext TodoContext { get; set; }

        public IQueryable<TEntity> FindAll()
        {
            return TodoContext.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression)
        {
            return TodoContext.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public IQueryable<TEntity> FindAll<TProperty>(Expression<Func<TEntity, bool>> expression, 
            Expression<Func<TEntity, TProperty>> with)
        {
            return TodoContext.Set<TEntity>().Include(with).Where(expression).AsNoTracking();
        }

        public async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return await TodoContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity?> FirstOrDefault<TProperty>(Expression<Func<TEntity, bool>> expression, 
            Expression<Func<TEntity, TProperty>> with)
        {
            return await TodoContext.Set<TEntity>().Include(with).AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task Add(TEntity entity)
        {
            await TodoContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            TodoContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            TodoContext.Set<TEntity>().Remove(entity);
        }
    }
}