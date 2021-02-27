using System.Collections.Generic;
using System.Threading.Tasks;
using Noteify.Data.Models;

namespace Noteify.Web.Areas.Account.Services.Interface
{
    public interface IManageService
    {
        Task<List<Note>> GetAllUndeletedNotes();
    }
}