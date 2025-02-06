using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Core.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public RepositoryBase(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        private ApplicationDbContext ApplicationDbContext { get; set; }

        public IQueryable<TEntity> FindAll()
        {
            return ApplicationDbContext.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression)
        {
            return ApplicationDbContext.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public IQueryable<TEntity> FindAll<TProperty>(Expression<Func<TEntity, bool>> expression, 
            Expression<Func<TEntity, TProperty>> with)
        {
            return ApplicationDbContext.Set<TEntity>().Include(with).Where(expression).AsNoTracking();
        }

        public async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return await ApplicationDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity?> FirstOrDefault<TProperty>(Expression<Func<TEntity, bool>> expression, 
            Expression<Func<TEntity, TProperty>> with)
        {
            return await ApplicationDbContext.Set<TEntity>().Include(with).AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task Add(TEntity entity)
        {
            await ApplicationDbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            ApplicationDbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            ApplicationDbContext.Set<TEntity>().Remove(entity);
        }
    }
}