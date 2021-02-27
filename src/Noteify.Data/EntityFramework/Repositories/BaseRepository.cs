using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Noteify.Data.EntityFramework.Interfaces;

namespace Noteify.Data.EntityFramework.Repositories
{
    /// <summary>
    ///     Interactions with the DbContext which are identical across all models.
    /// </summary>
    /// <typeparam name="TEntity">The model used</typeparam>
    /// <typeparam name="TKey">The data type of the primary key</typeparam>
    public abstract class BaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly NoteifyContext Context;
        protected readonly DbSet<TEntity> _entities;

        public BaseRepository(NoteifyContext context)
        {
            Context = context;
            _entities = Context.Set<TEntity>();
        }

        // GetAll
        public Task<List<TEntity>> GetAllAsync()
        {
            return _entities.AsNoTracking().ToListAsync();
        }

        // Get
        public Task<TEntity> GetAsync(TKey id)
        {
            return _entities.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        // Find
        public Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).AsNoTracking().ToListAsync();
        }

        // Add
        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        // Remove
        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        // Update
        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }
    }
}