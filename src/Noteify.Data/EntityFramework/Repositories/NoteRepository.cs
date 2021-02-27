using System;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.Models;

namespace Noteify.Data.EntityFramework.Repositories
{
    public class NoteRepository : BaseRepository<Note, Guid>, INoteRepository
    {
        public NoteRepository(NoteifyContext context) : base(context)
        { }

        // Add specific Db queries/manipulations here
    }
}