using System.Threading.Tasks;

namespace Noteify.Data.EntityFramework.Interfaces
{
    public interface IUnitOfWork
    {
        INoteRepository Notes { get; }

        Task BeginTransaction();
        Task Commit();
        void Rollback();
        Task<int> CompleteAsync();
        void Dispose();
    }
}