using System.Collections.Generic;
using System.Threading.Tasks;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.Models;
using Noteify.Web.Areas.Account.Helpers;
using Noteify.Web.Areas.Account.Services.Interface;

namespace Noteify.Web.Areas.Account.Services
{
    public class ManageService : IManageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<Note>> GetAllUndeletedNotes()
        {
            return _unitOfWork.Notes.FindAllAsync(x => !x.IsDeleted);
        }

        public static void AddSeparatorAndColumnHeaderToStringBuilder(ExportConfig ec)
        {
            ec.StringBuilder.AppendLine("sep=" + ec.Separator);
            ec.StringBuilder.AppendLine(string.Join(ec.Separator, ec.ColumnHeader));
        }

        public static void AddQuotedNotesToStringBuilder(ExportConfig ec)
        {
            ec.Content.ForEach(x =>
            {
                ec.StringBuilder.AppendLine(
                    Quote(x.Designation) + ec.Separator +
                    Quote(x.Message) + ec.Separator +
                    Quote(x.TimeStamp)
                );
            });
        }

        public static string Quote(dynamic propertie)
        {
            return $"\"{propertie}\"";
        }
    }
}