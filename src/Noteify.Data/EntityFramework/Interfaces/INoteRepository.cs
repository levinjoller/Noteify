using System;
using Noteify.Data.Models;

namespace Noteify.Data.EntityFramework.Interfaces
{
    public interface INoteRepository : IBaseRepository<Note, Guid>
    {
        // Add specific Db queries/manipulations here
    }
}