using System.ComponentModel.DataAnnotations;

namespace Noteify.Web.ViewModels.SaveViewModels
{
    public class NoteSVM
    {
        [Display(Name = "Titel")]
        [MaxLength(100, ErrorMessage = "Maximal 100 Zeichen erlaubt.")]
        [Required(ErrorMessage = "Das ist ein Pflichtfeld.")]
        public string Designation { get; set; }

        [Display(Name = "Inhalt")]
        [MaxLength(500, ErrorMessage = "Maximal 500 Zeichen erlaubt.")]
        [Required(ErrorMessage = "Das ist ein Pflichtfeld.")]
        public string Message { get; set; }
    }
}