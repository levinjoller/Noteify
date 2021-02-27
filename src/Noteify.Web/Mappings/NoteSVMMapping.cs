using Noteify.Data.Models;
using Noteify.Web.ViewModels.SaveViewModels;

namespace Noteify.Web.Mappings
{
    public static class NoteSVMMapping
    {
        public static Note CreateNote(NoteSVM noteSVM, string currentUserId)
        {
            return new Note()
            {
                Designation = noteSVM.Designation,
                Message = noteSVM.Message,
                UserId = currentUserId
            };
        }
    }
}