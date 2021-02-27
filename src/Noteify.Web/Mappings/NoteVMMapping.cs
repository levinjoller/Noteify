using System.Collections.Generic;
using Noteify.Data.Models;
using Noteify.Web.ViewModels;

namespace Noteify.Web.Mappings
{
    public static class NoteVMMapping
    {
        public static List<NoteViewModel> GetViewModel(List<Note> notes)
        {
            var noteViewModels = new List<NoteViewModel>();

            foreach (var note in notes)
            {
                noteViewModels.Add(
                    new NoteViewModel
                    {
                        Id = note.Id,
                        Designation = note.Designation,
                        Message = note.Message,
                        TimeStamp = note.TimeStamp
                    }
                );
            }
            return noteViewModels;
        }

        public static NoteViewModel GetViewModel(Note note)
        {
            return new NoteViewModel
            {
                Id = note.Id,
                Designation = note.Designation,
                Message = note.Message,
                TimeStamp = note.TimeStamp
            };
        }
    }
}