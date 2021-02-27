using System.Threading.Tasks;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.EntityFramework.Repositories;

namespace Noteify.Data.EntityFramework
{
    /// <summary>
    ///     Bundles stateful transactions and executes them together on the database (via the DbContext).
    ///     Manages the repositories and makes them available.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NoteifyContext _context;

        public UnitOfWork(NoteifyContext context)
        {
            _context = context;
            Notes = new NoteRepository(_context);
        }

        public INoteRepository Notes { get; private set; }

        public Task BeginTransaction()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public Task Commit()
        {
            return _context.Database.CommitTransactionAsync();
        }

        public void Rollback()
        {
            _context.Database.RollbackTransaction();
        }

        /// <summary>
        ///     Saves the changes to the database.
        /// </summary>
        /// <returns>
        ///     A task that represents the asynchronous save operation. 
        ///     The task result contains the number of state entries written to the database.
        /// </returns>
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}