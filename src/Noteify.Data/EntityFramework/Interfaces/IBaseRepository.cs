using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Noteify.Data.EntityFramework.Interfaces
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity> GetAsync(TKey id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}