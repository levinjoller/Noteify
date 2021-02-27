namespace Noteify.Web.Helpers
{
    public class NoteIndexFilter
    {
        public string Designation { get; set; }
        public string Date { get; set; }
        public bool DeletedOnly { get; set; } = false;
    }
}