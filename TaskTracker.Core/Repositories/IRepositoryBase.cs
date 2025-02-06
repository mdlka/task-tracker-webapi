using System.Linq.Expressions;

namespace TaskTracker.Core.Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        IQueryable<TEntity> FindAll();
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression);

        IQueryable<TEntity> FindAll<TProperty>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TProperty>> with);

        Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> expression);
        
        Task<TEntity?> FirstOrDefault<TProperty>(Expression<Func<TEntity, bool>> expression, 
            Expression<Func<TEntity, TProperty>> with);
        
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}