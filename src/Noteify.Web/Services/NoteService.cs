using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.Models;
using Noteify.Web.Helpers;
using Noteify.Web.Services.Interfaces;

namespace Noteify.Web.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Add(Note note)
        {
            _unitOfWork.Notes.Add(note);
            return _unitOfWork.CompleteAsync();
        }

        public Task<Note> GetNote(Guid id)
        {
            return _unitOfWork.Notes.GetAsync(id);
        }

        public Task<List<Note>> GetAllNotes()
        {
            return _unitOfWork.Notes.GetAllAsync();
        }

        public static Expression<Func<Note, bool>> GetNotesFilter(NoteIndexFilter filter)
        {
            var isDesignationEmpty = string.IsNullOrEmpty(filter.Designation);
            var isDateTimeEmpty = !DateTime.TryParse(filter.Date, out DateTime dateTime);

            // Pay attention to the order!
            return x =>
                (isDesignationEmpty || x.Designation.Equals(filter.Designation)) &&
                (isDateTimeEmpty || x.TimeStamp.Date == dateTime.Date) &&
                (x.IsDeleted == filter.DeletedOnly);
        }

        public Task<List<Note>> GetNotesFiltered(NoteIndexFilter filter)
        {
            return _unitOfWork.Notes.FindAllAsync(GetNotesFilter(filter));
        }

        public Task<int> Remove(Note note)
        {
            _unitOfWork.Notes.Remove(note);

            return _unitOfWork.CompleteAsync();
        }

        public Task<int> SoftRemove(Note note)
        {
            note.IsDeleted = true;
            _unitOfWork.Notes.Update(note);

            return _unitOfWork.CompleteAsync();
        }

        public Task<int> RestoreNote(Note note)
        {
            note.IsDeleted = false;
            _unitOfWork.Notes.Update(note);

            return _unitOfWork.CompleteAsync();
        }
    }
}