using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noteify.Data.Models;
using Noteify.Web.Helpers;

namespace Noteify.Web.Services.Interfaces
{
    public interface INoteService
    {
        Task<Note> GetNote(Guid id);
        Task<List<Note>> GetAllNotes();
        Task<List<Note>> GetNotesFiltered(NoteIndexFilter filter);
        Task<int> Add(Note note);
        Task<int> Remove(Note note);
        Task<int> SoftRemove(Note note);
        Task<int> RestoreNote(Note note);
    }
}