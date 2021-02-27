using System.Collections.Generic;
using System.Text;
using Noteify.Data.Models;

namespace Noteify.Web.Areas.Account.Helpers
{
    public class ExportConfig
    {
        public readonly StringBuilder StringBuilder;
        public readonly string Separator;
        public readonly string[] ColumnHeader;
        public readonly List<Note> Content;

        public ExportConfig(StringBuilder stringBuilder, string separator, string[] columnHeader, List<Note> content)
        {
            StringBuilder = stringBuilder;
            Separator = separator;
            ColumnHeader = columnHeader;
            Content = content;
        }
    }
}